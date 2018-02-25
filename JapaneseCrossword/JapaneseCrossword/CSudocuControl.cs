using JCClasses;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JapaneseСrossword
{
    public partial class CSudocuControl : UserControl
    {
        static object Locer = new object();
        public CSudocuControl()
        {
            InitializeComponent();
            _UpdateSudocuEvent = new UpdateSudocuEvent(_UpdateSudocu);
            _Sudocu = new Crossword();
            _Sudocu.SetSize(10,20);
        }

        //-----------------------------------------------------------------
        // Асинхронность, мать ее))
        public void DoSolve()
        {
            //
            // Делегат создается динамически, потому что для каждого объекта
            // должен быть свой делегат, вызывающий его перерисовку
            // Метод данного делегата не может быть статическим
            // (если я все правильно понимаю)
            // 
            CrosswordSolver Solver = new CrosswordSolver();
            Solver.ProgressEvent += RefreshProgress;
            Solver.DoSolve(_Sudocu);
        }

        /**
         * Делегат для вызова перерисовки окна из другой нити
         */
        private delegate void UpdateSudocuEvent();
        private UpdateSudocuEvent _UpdateSudocuEvent;

        /**
         * Функция делегата
         */
        private void _UpdateSudocu()
        { Refresh(); }



        /**
         * Данный метод передается классу, выполняющий решение сканворда
         * Аналог CallBack за исключением того что, при вызове данной функции
         * перерисовка контрола будет выполняться в основном потоке, а не в том
         * в котором была вызвана функция
         * (типа посылаем сообщение окну что ему нужно перерисоваться)
         */
        private void RefreshProgress()
        {
            Invoke(_UpdateSudocuEvent);
        }

        //-----------------------------------------------------------------
        public void LoadSudocu(String Path)
        {
            _Sudocu = Crossword.Load(Path);
            DoSolve();
        }


        public void SaveSudocu(String Path)
        {
            _Sudocu.Save(Path);
        }


        private void CSudocuControl_Paint(object sender, PaintEventArgs e)
        {
            SuspendLayout();
            if (_Sudocu == null)
                return;
            this.Size = CalcSize();
            Pen pencil = new Pen(Color.Black);
            LinearGradientBrush Gradient =
                new LinearGradientBrush(
                                 new Point(0, 0),
                                 new Point(0, Size.Height),
                                 Color.FromArgb(50, 192, 192, 255),
                                 Color.FromArgb(50, 255, 0, 255));

            e.Graphics.FillRectangle(
                Gradient,
                0,
                0,
                Size.Width,
                Size.Height);

            //-------------------------------------------------------------
            //Рисуем линии параллельные оси Y
            for (byte i = 0; i < this._Sudocu.Size.Width + 1; i++)
            {
                if (i % 5 == 0)
                    pencil.Width = SeparatorSize;
                else
                    pencil.Width = 1;

                Int32 YLine = GetYLocationVerticalLine(i);
                e.Graphics.DrawLine(
                    pencil,
                    YLine,
                    0,
                    YLine,
                    Size.Height);
            }

            //-------------------------------------------------------------
            //Рисуем линии параллельные оси X
            for (byte i = 0; i < this._Sudocu.Size.Height + 1; i++)
            {
                if (i % 5 == 0)
                    pencil.Width = SeparatorSize;
                else
                    pencil.Width = 1;

                Int32 XLine = GetXLocationHorizontalLine(i);
                e.Graphics.DrawLine(pencil, 0, XLine, Size.Width, XLine);
            }

            //=============================================================
            // Рисуем исходные данные по горизонтали
            Font font = new Font(
                "Microsoft Sans Serif",
                8F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(204)));

            Brush brush = new SolidBrush(Color.Black);

            for (byte i = 0; i < this._Sudocu.Size.Width; i++)
            {
                for (byte j = 0; j < this._Sudocu.Horizontal[i].GetCount(); j++)
                {
                    e.Graphics.DrawString(
                        this._Sudocu.Horizontal[i].list[j].ToString(),
                        font,
                        brush,
                        GetLocationHorizontalValue(i, j));
                }
            }

            //-------------------------------------------------------------
            // По вертикали
            for (byte i = 0; i < this._Sudocu.Size.Height; i++)
            {
                for (byte j = 0; j < _Sudocu.Vertical[i].GetCount(); j++)
                {
                    e.Graphics.DrawString(
                        this._Sudocu.Vertical[i].list[j].ToString(),
                        font,
                        brush,
                        GetLocationVerticalValue(j, i));
                }
            }

            //-------------------------------------------------------------
            //Заполняем сетку
            for (byte i = 0; i < _Sudocu.Size.Width; i++)
            {
                Int32 X;
                Int32 Y;
                for (byte j = 0; j < _Sudocu.Size.Height; j++)
                {
                    if (0 == _Sudocu.GetCell(i, j))
                    {
                        continue;
                    }
                    X = GetYLocationVerticalLine(i) + 2;
                    Y = GetXLocationHorizontalLine(j) + 2;

                    if (0 == (i % 5))
                        X += SeparatorSize / 2;
                    if (0 == (j % 5))
                        Y += SeparatorSize / 2;

                    if (100 == _Sudocu.GetCell(i, j))
                    {
                        Gradient = new LinearGradientBrush(
                                new Point(X, Y),
                                new Point(X + CellSize - 1, Y + CellSize - 1),
                                Color.FromArgb(255, 0, 0, 250),
                                Color.FromArgb(150, 0, 0, 250));

                    }
                    else
                    {
                        Gradient = new LinearGradientBrush(
                                new Point(X, Y),
                                new Point(X + CellSize - 1, Y + CellSize - 1),
                                Color.FromArgb(255, 0, 0, 0),
                                Color.FromArgb(150, 0, 0, 0));
                    }
                    

                    e.Graphics.FillRectangle(
                        Gradient,
                        X,
                        Y,
                        CellSize - 3,
                        CellSize - 3);
                }
            }
            //------------------------------------------------------------------------------
            ResumeLayout();
        }


        public byte CellSize
        {
            get{ return _CellSize; }
            set{ _CellSize = value; }
        }


        public byte SeparatorSize
        { 
            get{ return _SeparatorSize; }
            set
            {
                _SeparatorSize = value;
                _SeparatorSize += (byte)(1 - (_SeparatorSize % 2));
            }
        }


        private Size CalcSize()
        {
            Size size = new Size();

            size.Height = (_Sudocu.Horizontal.GetMaxCount() + _Sudocu.Size.Height) * CellSize + _Sudocu.Size.Height / _BoxSize * (SeparatorSize - 1) + SeparatorSize;
            size.Width = (_Sudocu.Vertical.GetMaxCount() + _Sudocu.Size.Width) * CellSize + _Sudocu.Size.Width / _BoxSize * (SeparatorSize - 1) + SeparatorSize;
            return size;
        }


        private Int32 GetXLocationHorizontalLine(Int32 Index)
        {
            return (Index * CellSize + ((Index / _BoxSize + (Index - 1) / _BoxSize) + ((Index != 0) ? 1 : 0)) * (SeparatorSize / 2) + _Sudocu.Horizontal.GetMaxCount() * CellSize) + (SeparatorSize / 2);
        }


        private Int32 GetYLocationVerticalLine(Int32 Index)
        {
            return (Index * CellSize + ((Index / _BoxSize + (Index - 1) / _BoxSize) + ((Index != 0) ? 1 : 0)) * (SeparatorSize / 2) + _Sudocu.Vertical.GetMaxCount() * CellSize) + (SeparatorSize / 2);
        }


        private Point GetLocationHorizontalValue(Int32 X,Int32 Y)
        {
            Point point = new Point();
            point.X = GetYLocationVerticalLine(X) + ((X % _BoxSize == 0) ? 1 : 0) * (SeparatorSize / 2);
            point.Y = CellSize * (_Sudocu.Horizontal.GetMaxCount() - _Sudocu.Horizontal[X].GetCount() + Y);
            return point;
        }


        private Point GetLocationVerticalValue(Int32 X, Int32 Y)
        {
            Point point = new Point();
            point.X = CellSize * (_Sudocu.Vertical.GetMaxCount() - _Sudocu.Vertical[Y].GetCount() + X);
            point.Y = GetXLocationHorizontalLine(Y) + ((Y % 5 == 0) ? 1 : 0) * (SeparatorSize / 2);
            return point;
        }


        private byte GetColumn(Int32 dwXPix)
        {
            Int32 X;
            Int32 cl;
            Int32 cr;
            byte res = 0xFF;

            if (dwXPix > CalcSize().Width)
                return res;
            if (dwXPix < ((_Sudocu.Vertical.GetMaxCount() * CellSize) + SeparatorSize))
                return 0xFE;


            dwXPix -= (_Sudocu.Vertical.GetMaxCount() * CellSize) + SeparatorSize;


            X = (dwXPix) / (CellSize * _BoxSize + (SeparatorSize));

            cl = X * (_BoxSize * CellSize) + X * SeparatorSize - 1;

            cr = (X + 1) * (_BoxSize * CellSize) + X * (SeparatorSize);
            if ((dwXPix > cl) && (dwXPix < cr))
                res = (byte)((dwXPix - cl) / CellSize + X * _BoxSize);

            return res;
        }


        //-----------------------------------------------------------------
        private byte GetRow(Int32 dwYPix)
        {
            Int32 X;
            Int32 cl;
            Int32 cr;
            byte res = 0xFF;

            if (dwYPix > CalcSize().Height)
                return res;
            if (dwYPix < (_Sudocu.Horizontal.GetMaxCount() * CellSize))
                return 0xFE;

            dwYPix -= (_Sudocu.Horizontal.GetMaxCount() * CellSize) + SeparatorSize;


            X = (dwYPix) / (CellSize * 5 + (SeparatorSize));

            cl = X * (_BoxSize * CellSize) + X * SeparatorSize;

            cr = (X + 1) * (_BoxSize * CellSize) + X * SeparatorSize - 1;
            if ((dwYPix > cl) && (dwYPix < cr))
                res = (byte)((dwYPix - cl) / CellSize + X * _BoxSize);

            return res;
        }


        private void CSudocuControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (_Sudocu == null)
                return;

            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    _Sudocu.SetCell(GetColumn(e.X), GetRow(e.Y), 1);
                }
                if (e.Button == MouseButtons.Right)
                {
                    _Sudocu.SetCell(GetColumn(e.X), GetRow(e.Y), 0);
                }
                Refresh();
            }
            catch (ArgumentOutOfRangeException)
            { }
        }


        private void CSudocuControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Sudocu == null)
                return;

            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    _Sudocu.SetCell(GetColumn(e.X), GetRow(e.Y), 1);
                }
                if (e.Button == MouseButtons.Right)
                {
                    _Sudocu.SetCell(GetColumn(e.X), GetRow(e.Y), 0);
                }
                Refresh();
            }
            catch(ArgumentOutOfRangeException)
            { }
        }


        private Crossword _Sudocu = null;
        private byte _CellSize = 15;
        private byte _SeparatorSize = 10;
        const byte _BoxSize = 5;

    }
}
