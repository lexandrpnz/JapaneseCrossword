using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;


namespace SudocuClsses
{
    /**
     * Класс описывает японский сканворд
     * содержит игровое поле и матрицы исходных данных
     */
    [Serializable()]
    [XmlRoot("Sudocu")]
    public class CSudocu: CGrid
    {
        public CSudocu()
        { }

        ~CSudocu()
        { }

        /**
         * Матрица исходных данных по вертикали
         */
        public NumericalMatrix Vertical
        {
            get
            {
                lock (_Locked)
                {
                    return m_VerticalMatrix;
                }
            }
            //set{ m_VerticalMatrix = value; }
        }

        /**
         * Матрица исходных данных по горихонтали
         */
        public NumericalMatrix Horizontal
        {
            get
            {
                lock (_Locked)
                {
                    return _HorizontalMatrix;
                }
            }
            //set{ m_HorizontalMatrix = value; }
        }

        /**
         * Установить размер игрвого поля
         */
        public void SetSize(byte Width,byte Height)
        {
            lock (_Locked)
            {
                this.Size = new Size(Width, Height);
                m_VerticalMatrix.Clear();
                byte i;
                for (i = 0; i < Height; i++)
                {
                    m_VerticalMatrix.Add(new ByteList());
                }
                _HorizontalMatrix.Clear();
                for (i = 0; i < Width; i++)
                {
                    _HorizontalMatrix.Add(new ByteList());
                }
            }
        }


        public void Load(String Path)
        {
            lock (_Locked)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CSudocu));
                Stream stream = new FileStream(Path, FileMode.Open);

                CSudocu Sudocu = (CSudocu)serializer.Deserialize(stream);
                Size = Sudocu.Size;
                _HorizontalMatrix = Sudocu.Horizontal;
                m_VerticalMatrix = Sudocu.Vertical;
                value = Sudocu.value;

                stream.Close();
            }
        }


        public void Save(String Path)
        {
            lock (_Locked)
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(CSudocu));
                Stream stream = new FileStream(Path, FileMode.Create);

                Serializer.Serialize(stream, this, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) }));
                stream.Close();
            }
        }


        private NumericalMatrix m_VerticalMatrix = new NumericalMatrix();
        private NumericalMatrix _HorizontalMatrix = new NumericalMatrix();
        private Object _Locked = new Object();
    }
}

