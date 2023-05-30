using System; // подключаем пространство имен System

using System.Text; // подключаем пространство имен System.Text

using System.Windows.Forms; // подключаем пространство имен System.Windows.Forms

namespace lab12_pudov // объявляем пространство имён WindowsFormsApplication4
{
    public partial class Form1 : Form // создаём класс формы Form1, наследуемый от Form
    {
        private Model model; // объявляем закрытое поле модели

        public Form1() // объявляем конструктор класса Form1
        {
            InitializeComponent(); // вызываем метод InitializeComponent для инициализации компонентов формы
            model = new Model(); // создаем экземпляр класса Model и назначаем его полю model
            InitFormAndModel(); // вызываем метод InitFormAndModel для начальной инициализации формы и модели
        }

        private void Launch_Click(object sender, EventArgs e) // объявляем обработчик событий кнопки Launch
        {
            var button = sender as Button; // приводим объект-источник события к типу Button

            switch (button.Text) // запускаем оператор switch на основе текста кнопки
            {
                case "Старт": // если текст кнопки "Старт"
                    Run(); // вызываем метод Run()
                    break;
                case "Продолжить": // если текст кнопки "Продолжить"
                    Continue(); // вызываем метод Continue()
                    break;
                case "Пауза": // если текст кнопки "Пауза"
                    Pause(); // вызываем метод Pause()
                    break;
            }
        }

        private void Stop_Click(object sender, EventArgs e) // объявляем обработчик событий кнопки Stop
        {
            Stop(); // вызываем метод Stop()
        }

        private void Timer_Tick(object sender, EventArgs e) // объявляем обработчик событий таймера Timer
        {
            model.NextTick(); // вызываем метод NextTick() у модели
            UpdateForm(); // вызываем метод UpdateForm()
        }

        private void InitFormAndModel() // объявляем метод InitFormAndModel()
        {
            model.Init(); // вызываем метод Init() у модели

            UpdateForm(); // вызываем метод UpdateForm()
        }

        private void UpdateForm() // объявляем метод UpdateForm()
        {
            WeatherLabel.Text = $"Погода сейчас: {GetLabel(model.states[model.states.Count - 2])}"; // задаем текст метки WeatherLabel

            var time = (int)Math.Ceiling(model.t * 10); // вычисляем значение времени в днях
            TimeLabel.Text = $"Прошло: {time / 100}.{time % 100} дней"; // задаем текст метки TimeLabel

            var str = new StringBuilder("История:"); // создаем экземпляр класса StringBuilder для формирования строки истории
            for (int i = model.states.Count - 3; i >= Math.Max(0, model.states.Count - 8); --i) // перебираем элементы списка состояний модели
            {
                time = (int)Math.Ceiling(model.times[i] * 10); // вычисляем значение времени в днях для текущего элемента
                str.Append($" {{ {GetLabel(model.states[i])}, {time / 100}.{time % 100} }}"); // добавляем информацию об элементе в строку истории
            }
            HistoryLabel.Text = str.ToString(); // задаем текст метки HistoryLabel

            FineLabel.Text = $"было %ясных дней = {(100 * model.stateTimes[0] / model.times[model.times.Count - 1]).ToString("f1")} (теоретически 38%)"; // задаем текст метки FineLabel
            CloudyLabel.Text = $"было %облачных дней = {(100 * model.stateTimes[1] / model.times[model.times.Count - 1]).ToString("f1")} (теоретически 30%)"; // задаем текст метки CloudyLabel
            RainyLabel.Text = $"было %пасмурных дней = {(100 * model.stateTimes[2] / model.times[model.times.Count - 1]).ToString("f1")} (теоретически 32%)"; // задаем текст метки RainyLabel
        }

        private string GetLabel(WeatherState state) // объявляем метод GetLabel()
        {
            switch (state) // запускаем оператор switch на основе переданного состояния
            {
                case WeatherState.Fine: return "ясно"; // если состояние "ясный"
                case WeatherState.Cloudy: return "облачно"; // если состояние "облачный"
                case WeatherState.Rainy: return "пасмурно"; // если состояние "пасмурный"
            }

            return ""; // возвращаем пустую строку
        }

        private void Run() // объявляем метод Run()
        {
            InitFormAndModel(); // вызываем метод InitFormAndModel() для начальной инициализации формы и модели
            LaunchBtn.Text = "Пауза"; // меняем текст кнопки LaunchBtn на "Пауза"
            StopBtn.Enabled = true; // активируем кнопку StopBtn
            Timer.Enabled = true; // запускаем таймер Timer
        }

        private void Pause() // объявляем метод Pause()
        {
            LaunchBtn.Text = "Продолжить"; // меняем текст кнопки LaunchBtn на "Продолжить"
            Timer.Enabled = false; // останавливаем таймер Timer
        }

        private void Continue() // объявляем метод Continue()
        {
            LaunchBtn.Text = "Пауза"; // меняем текст кнопки LaunchBtn на "Пауза"
            Timer.Enabled = true; // запускаем таймер Timer
        }

        private void Stop() // объявляем метод Stop()
        {
            LaunchBtn.Text = "Старт"; // меняем текст кнопки LaunchBtn на "Старт"
            StopBtn.Enabled = false; // дезактивируем кнопку StopBtn
            Timer.Enabled = false; // останавливаем таймер Timer
        }

        private void trackBar1_Scroll(object sender, EventArgs e) // объявляем обработчик событий трекбара trackBar1
        {
            var trackBar = sender as TrackBar; // приводим объект-источник события к типу TrackBar

            switch (trackBar.Value) // запускаем оператор switch на основе значения трекбара
            {
                case 0: // если значение 0
                    model.dt = 0.25; // задаем значение модельного времени dt равным 0.25
                    break;
                case 1: // если значение 1
                    model.dt = 0.5; // задаем значение модельного времени dt равным 0.5
                    break;
                case 2: // если значение 2
                    model.dt = 1; // задаем значение модельного времени dt равным 1
                    break;
                case 3: // если значение 3
                    model.dt = 4; // задаем значение модельного времени dt равным 4
                    break;
                case 4: // если значение 4
                    model.dt = 16; // задаем значение модельного времени dt равным 16
                    break;
            }
        }
    }
}