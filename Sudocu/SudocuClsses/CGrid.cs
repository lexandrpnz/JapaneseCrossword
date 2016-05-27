using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace SudocuClsses
{
    /**
     * ����� ��������� ������ ���� ��������� 
     * ������ �������� ���� �������� ���� ��� ��� �������� �������
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
          * ������ �������� ����
          */
        public Size Size
        {
            get
            {
                lock (_Locked)
                {
                    return m_Size;
                }
            }
            set
            {
                lock (_Locked)
                {
                    m_Size = value;
                    m_pGrid = new Byte[m_Size.Width * m_Size.Height];
                }
            }
        }

        /**
         * �������� �������� ������ � ������������ cX � cY
         */
        public byte GetCell(byte cX, byte cY)
        {
            if ((Size.Width <= cX) || (Size.Height <= cY))
                throw new ArgumentOutOfRangeException();

            return value[(Size.Width * cY) + cX];
        }

        /**
         * ������ �������� ������ � ������������ cX � cY
         */
        public void SetCell(byte cX, byte cY, byte cValue)
        {
            if ((Size.Width <= cX) || (Size.Height <= cY))
                throw new ArgumentOutOfRangeException();
            value[(Size.Width * cY) + cX] = cValue;
        }

        /**
         * ����
         */
        public Byte[] value
        {
            get
            {
                lock (_Locked)
                {
                    return m_pGrid; 
                }
            }
            set
            {
                lock (_Locked)
                {
                    m_pGrid = value;
                }
            }
        }

        private Size	m_Size;
 	    private Byte[]	m_pGrid;
        private Object _Locked = new Object();
    }
}
