using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace SudocuClsses
{
    /**
     * Класс описывает игрове поле сканворда 
     * Размер игрового поля задается один раз при создании объекта
     */
    [Serializable()]
    [XmlRoot("Grid")]
    public class CGrid
    {
        public CGrid()
        { }

        ~CGrid( )
        { }

         /**
          * Размер игрового поля
          */
        public Size Size
        {
            get
            {
                lock (lockeObj)
                {
                    return m_Size;
                }
            }
            set
            {
                lock (lockeObj)
                {
                    m_Size = value;
                    m_pGrid = new Byte[m_Size.Width * m_Size.Height];
                }
            }
        }

        /**
         * Получить значение ячейки с координатами x и cY
         */
        public byte GetCell(byte x, byte y)
        {
            if ((Size.Width <= x) || (Size.Height <= y))
                throw new ArgumentOutOfRangeException();

            return value[(Size.Width * y) + x];
        }

        /**
         * Задать значение ячейки с координатами x и cY
         */
        public void SetCell(byte x, byte y, byte value)
        {
            if ((Size.Width <= x) || (Size.Height <= y))
                throw new ArgumentOutOfRangeException();
            this.value[(Size.Width * y) + x] = value;
        }

        /**
         * Поле
         */
        public Byte[] value
        {
            get
            {
                lock (lockeObj)
                {
                    return m_pGrid; 
                }
            }
            set
            {
                lock (lockeObj)
                {
                    m_pGrid = value;
                }
            }
        }

        public Byte this[byte nX, byte nY]
        {
            get
            {
                return GetCell(nX, nY);
            }
            set
            {
                SetCell(nX, nY, value);
            }
        }

        private Size	m_Size;
 	    private Byte[]	m_pGrid;
        private Object lockeObj = new Object();
    }
}
