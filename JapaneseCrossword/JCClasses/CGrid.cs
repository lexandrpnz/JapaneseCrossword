using System;
using System.Drawing;
using System.Xml.Serialization;

namespace JCClasses
{
    /**
     * ����� ��������� ������ ���� ��������� 
     * ������ �������� ���� �������� ���� ��� ��� �������� �������
     */
    [Serializable()]
    [XmlRoot("Grid")]
    public class CGrid
    {
         /**
          * ������ �������� ����
          */
        public Size Size
        {
            get
            {
                lock (lockObj)
                {
                    return m_Size;
                }
            }
            set
            {
                lock (lockObj)
                {
                    m_Size = value;
                    m_pGrid = new Byte[m_Size.Width * m_Size.Height];
                }
            }
        }

        /**
         * �������� �������� ������ � ������������ x � cY
         */
        public byte GetCell(byte x, byte y)
        {
            if ((Size.Width <= x) || (Size.Height <= y))
                throw new ArgumentOutOfRangeException();

            return value[(Size.Width * y) + x];
        }

        /**
         * ������ �������� ������ � ������������ x � cY
         */
        public void SetCell(byte x, byte y, byte value)
        {
            if ((Size.Width <= x) || (Size.Height <= y))
                throw new ArgumentOutOfRangeException();
            this.value[(Size.Width * y) + x] = value;
        }

        /**
         * ����
         */
        public Byte[] value
        {
            get
            {
                lock (lockObj)
                {
                    return m_pGrid; 
                }
            }
            set
            {
                lock (lockObj)
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
        private Object lockObj = new Object();
    }
}
