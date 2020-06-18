using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabTwo
{
    public partial class LabTwoForm : Form
    {
        public LabTwoForm()
        {
            InitializeComponent();
            // заполнение стилями линий
            string[] dashNameArray = Enum.GetNames(typeof(System.Drawing.Drawing2D.DashStyle));
            for (int i = 0; i < dashNameArray.Length - 1; i++)
                comboBox1.Items.Add(dashNameArray[i]);
            comboBox1.SelectedIndex = 0;
        }

        int[,] kv = new int[4, 3]; // матрица тела
        int[,] fig = new int[4, 3]; // матрица тела по варианту
        int[,] osi = new int[4, 3]; // матрица координат осей
        float[,] matr_sdv = new float[3, 3]; //матрица преобразования

        //инициализация матрицы тела примера
        private void Init_kvadrat()
        {
            //      X            Y
            kv[0, 0] = -50; kv[0, 1] = 0;   kv[0, 2] = 1;
            kv[1, 0] = 0;   kv[1, 1] = 50;  kv[1, 2] = 1;
            kv[2, 0] = 50;  kv[2, 1] = 0;   kv[2, 2] = 1;
            kv[3, 0] = 0;   kv[3, 1] = -50; kv[3, 2] = 1;
        }

        //вывод квадрата на экран
        private void Draw_Kv(Graphics graphics)
        {
            Init_kvadrat(); //инициализация матрицы тела
            Init_matr_preob(k, l, k1, l1, phi); //инициализация матрицы преобразования
            float[,] kv1 = Multiply_matr(kv, matr_sdv); //перемножение матриц
            using (Pen myPen = new Pen(Color.Blue, 2)) // цвет линии и ширина
            {
                //создаем новый объект Graphics (поверхность рисования) из pictureBox
                Graphics g = graphics; // Graphics.FromHwnd(pictureBox1.Handle);
                                       // рисуем 1 сторону квадрата
                g.DrawLine(myPen, kv1[0, 0], kv1[0, 1], kv1[1, 0], kv1[1, 1]);
                // рисуем 2 сторону квадрата
                g.DrawLine(myPen, kv1[1, 0], kv1[1, 1], kv1[2, 0], kv1[2, 1]);
                // рисуем 3 сторону квадрата
                g.DrawLine(myPen, kv1[2, 0], kv1[2, 1], kv1[3, 0], kv1[3, 1]);
                // рисуем 4 сторону квадрата
                g.DrawLine(myPen, kv1[3, 0], kv1[3, 1], kv1[0, 0], kv1[0, 1]);
            }
        }

        //инициализация матрицы тела по варианту
        private void Init_figure()
        {
            //      X                   Y
            fig[0, 0] = -100;    fig[0, 1] = -50;   fig[0, 2] = 1;
            fig[1, 0] = 150;     fig[1, 1] = 150;   fig[1, 2] = 1;
            fig[2, 0] = 150;     fig[2, 1] = -150;  fig[2, 2] = 1;
            fig[3, 0] = -100;    fig[3, 1] = 50;    fig[3, 2] = 1;
        }

        //вывод фигуры по варианту на экран
        private void Draw_Figure(Graphics graphics)
        {
            Init_figure(); //инициализация матрицы тела
            Init_matr_preob(k, l, k1, l1, phi); //инициализация матрицы преобразования
            float[,] fig1 = Multiply_matr(fig, matr_sdv); //перемножение матриц
            using (Pen myPen = new Pen(label6.BackColor, 2)) // цвет линии и ширина
            {
                myPen.DashStyle = dashStyle;
                Graphics g = graphics;
                
                g.DrawLine(myPen, fig1[0, 0], fig1[0, 1], fig1[1, 0], fig1[1, 1]);
                
                g.DrawLine(myPen, fig1[1, 0], fig1[1, 1], fig1[2, 0], fig1[2, 1]);
                
                g.DrawLine(myPen, fig1[2, 0], fig1[2, 1], fig1[3, 0], fig1[3, 1]);
                
                g.DrawLine(myPen, fig1[3, 0], fig1[3, 1], fig1[0, 0], fig1[0, 1]);
            }
        }

        //инициализация матрицы сдвига
        private void Init_matr_preob(int k1, int l1, float x1 = 1f, float y1 = 1f, float phi = 0f)
        {
            float cf = (float)Math.Cos(phi);
            float sf = (float)Math.Sin(phi);
            matr_sdv[0, 0] = x1 * cf;   matr_sdv[0, 1] = sf;        matr_sdv[0, 2] = 0;
            matr_sdv[1, 0] = -sf;       matr_sdv[1, 1] = y1 * cf;   matr_sdv[1, 2] = 0;
            matr_sdv[2, 0] = k1;        matr_sdv[2, 1] = l1;        matr_sdv[2, 2] = 1;
        }

        //инициализация матрицы осей
        private void Init_osi()
        {
            osi[0, 0] = -200;   osi[0, 1] = 0;      osi[0, 2] = 1;
            osi[1, 0] = 200;    osi[1, 1] = 0;      osi[1, 2] = 1;
            osi[2, 0] = 0;      osi[2, 1] = 200;    osi[2, 2] = 1;
            osi[3, 0] = 0;      osi[3, 1] = -200;   osi[3, 2] = 1;

        }

        //умножение матриц
        private float[,] Multiply_matr(int[,] a, float[,] b)
        {
            int n = a.GetLength(0);
            int m = a.GetLength(1);

            float[,] r = new float[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    r[i, j] = 0;
                    for (int ii = 0; ii < m; ii++)
                    {
                        r[i, j] += a[i, ii] * b[ii, j];
                    }
                }
            }
            return r;
        }

        int k0, l0; // элементы начала координат
        int k, l; // элементы матрицы сдвига
        float k1 = 1, l1 = 1; // элементы матрицы отражения

        //вывод осей на экран
        private void Draw_osi(Graphics graphics)
        {
            Init_osi();
            Init_matr_preob(k0, l0);
            float[,] osi1 = Multiply_matr(osi, matr_sdv);
            Pen myPen = new Pen(Color.Red, 1);// цвет линии и ширина
            Graphics g = graphics; // Graphics.FromHwnd(pictureBox1.Handle);
            // рисуем ось ОХ
            g.DrawLine(myPen, osi1[0, 0], osi1[0, 1], osi1[1, 0], osi1[1, 1]);
            // рисуем ось ОУ
            g.DrawLine(myPen, osi1[2, 0], osi1[2, 1], osi1[3, 0], osi1[3, 1]);
            //g.Dispose();
            myPen.Dispose();
        }

        //вывод осей в центре pictureBox
        private void button1_Click(object sender, EventArgs e)
        {
            k0 = pictureBox1.Width / 2;
            l0 = pictureBox1.Height / 2;
            show_osi = true;
            pictureBox1.Invalidate();
        }

        //вывод квадратика в центре pictureBox
        private void button2_Click(object sender, EventArgs e)
        {
            //середина pictureBox
            k = pictureBox1.Width / 2;
            l = pictureBox1.Height / 2;
            //вывод квадратика в середине
            //Draw_Kv();
            show_Kv = true;
            pictureBox1.Invalidate();
        }

        //очистить pictureBox
        private void button3_Click(object sender, EventArgs e)
        {
            k = pictureBox1.Width / 2;
            l = pictureBox1.Height / 2;
            k1 = 1;
            l1 = 1;
            phi = 0;
            dashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            Clear_All();
        }

        //очистка поля
        private void Clear_All()
        {
            //Init_osi();
            //Init_matr_preob(k, l);
            //int[,] osi1 = Multiply_matr(osi, matr_sdv);
            //Brush myBrush = new SolidBrush(SystemColors.Control);// цвет линии и ширина
            ////создаем новый объект Graphics (поверхность рисования) из pictureBox
            //Graphics g = Graphics.FromHwnd(pictureBox1.Handle);
            //Rectangle rect = new Rectangle(osi1[0, 0], osi1[3, 1], osi1[1, 0] - osi1[0, 0], osi1[2, 1] - osi1[3, 1]);
            //g.FillRectangle(myBrush, rect);
            //g.Dispose();// освобождаем все ресурсы, связанные с отрисовкой
            //myBrush.Dispose();
            show_osi = false;
            show_Kv = false;
            pictureBox1.Invalidate();
        }

        //сдвиг вправо
        private void button4_Click(object sender, EventArgs e)
        {
            k += 5; //изменение соответствующего элемента матрицы сдвига
            //Draw_Kv(); //вывод квадратика
            direction = MoveDirection.Right; //запоминаем направление движения
            pictureBox1.Invalidate();
        }

        //сдвиг влево
        private void button5_Click(object sender, EventArgs e)
        {
            k -= 5; //изменение соответствующего элемента матрицы сдвига
            //Draw_Kv(); //вывод квадратика
            direction = MoveDirection.Left; //запоминаем направление движения
            pictureBox1.Invalidate();
        }

        //сдвиг вниз
        private void button6_Click(object sender, EventArgs e)
        {
            l += 5; //изменение соответствующего элемента матрицы сдвига
            //Draw_Kv(); //вывод квадратика
            direction = MoveDirection.Down; //запоминаем направление движения
            pictureBox1.Invalidate();
        }

        //сдвиг вверх
        private void button7_Click(object sender, EventArgs e)
        {
            l -= 5; //изменение соответствующего элемента матрицы сдвига
            //Draw_Kv(); //вывод квадратика
            direction = MoveDirection.Up; //запоминаем направление движения
            pictureBox1.Invalidate();
        }

        bool f = true;

        //непрерывное перемещение
        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Interval = 100;

            button8.Text = "Стоп";
            if (f == true)
                timer1.Start();
            else
            {
                timer1.Stop();
                button8.Text = "Старт";
            }
            f = !f;
        }

        enum MoveDirection
        {
            Right,
            Left,
            Down,
            Up,
            Rotate
        }

        MoveDirection direction = MoveDirection.Right; //поумолчанию движемся вправо

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (direction)
            {
                case MoveDirection.Right:
                    k++;
                    break;
                case MoveDirection.Left:
                    k--;
                    break;
                case MoveDirection.Down:
                    l++;
                    break;
                case MoveDirection.Up:
                    l--;
                    break;
                case MoveDirection.Rotate:
                    phi += 0.01f;
                    break;
            }
            //Draw_Kv();
            //Thread.Sleep(100);
            pictureBox1.Invalidate();
        }

        //отражение по оси OX
        private void button9_Click(object sender, EventArgs e)
        {
            l1 = -l1;
            pictureBox1.Invalidate();
        }

        //отражение по оси OY
        private void button10_Click(object sender, EventArgs e)
        {
            k1 = -k1;
            pictureBox1.Invalidate();
        }

        // уменьшить масштаб по оси X
        private void button11_Click(object sender, EventArgs e)
        {
            k1 = k1 * 0.9f;
            pictureBox1.Invalidate();
        }

        // увеличить масштаб по оси X
        private void button12_Click(object sender, EventArgs e)
        {
            k1 = k1 * 1.1f;
            pictureBox1.Invalidate();
        }

        // уменьшить масштаб по оси Y
        private void button13_Click(object sender, EventArgs e)
        {
            l1 = l1 * 0.9f;
            pictureBox1.Invalidate();
        }

        // увеличить масштаб по оси Y
        private void button14_Click(object sender, EventArgs e)
        {
            l1 = l1 * 1.1f;
            pictureBox1.Invalidate();
        }

        float phi = 0f;

        // поворот фигуры
        private void button15_Click(object sender, EventArgs e)
        {
            phi += 0.01f;
            direction = MoveDirection.Rotate; //запоминаем направление движения
            pictureBox1.Invalidate();
        }

        // выбор цвета линии
        private void label6_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = label6.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                label6.BackColor = colorDialog1.Color;
                pictureBox1.Invalidate();
            }
        }

        //рисуем тип линии
        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var g = e.Graphics;
            // Draw the background of the item.
            e.DrawBackground();
            var rect = new Rectangle(e.Bounds.X, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
            try
            {
                rect.Inflate(-4, 0);
                using (var p = new Pen(e.ForeColor))
                {
                    p.Width = 2;
                    p.DashStyle = (System.Drawing.Drawing2D.DashStyle)(e.Index);
                    g.DrawLine(p, new Point(rect.Left, rect.Top + rect.Height / 2),
                                  new Point(rect.Right, rect.Top + rect.Height / 2));
                }
            }
            catch { }
            // Draw the focus rectangle if the mouse hovers over an item.
            e.DrawFocusRectangle();
        }

        System.Drawing.Drawing2D.DashStyle dashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;
            dashStyle = (System.Drawing.Drawing2D.DashStyle)cb.SelectedIndex;
            pictureBox1.Invalidate();
        }

        bool show_osi = false;

        bool show_Kv = false;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (show_osi)
                Draw_osi(e.Graphics);
            if (show_Kv)
                Draw_Figure(e.Graphics);
        }
    }
}
