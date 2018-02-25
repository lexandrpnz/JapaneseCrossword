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
                lock (locket)
                {
                    return verticalMatrix;
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
                lock (locket)
                {
                    return horizontalMatrix;
                }
            }
            //set{ m_HorizontalMatrix = value; }
        }

        /**
         * Установить размер игрвого поля
         */
        public void SetSize(byte Width,byte Height)
        {
            lock (locket)
            {
                this.Size = new Size(Width, Height);
                verticalMatrix.Clear();
                byte i;
                for (i = 0; i < Height; i++)
                {
                    verticalMatrix.Add(new ByteList());
                }
                horizontalMatrix.Clear();
                for (i = 0; i < Width; i++)
                {
                    horizontalMatrix.Add(new ByteList());
                }
            }
        }


        public void Load(String Path)
        {
            lock (locket)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CSudocu));
                Stream stream = new FileStream(Path, FileMode.Open);

                CSudocu Sudocu = (CSudocu)serializer.Deserialize(stream);
                Size = Sudocu.Size;
                horizontalMatrix = Sudocu.Horizontal;
                verticalMatrix = Sudocu.Vertical;
                value = Sudocu.value;

                stream.Close();
            }
        }


        public void Save(String Path)
        {
            lock (locket)
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(CSudocu));
                Stream stream = new FileStream(Path, FileMode.Create);

                Serializer.Serialize(stream, this, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) }));
                stream.Close();
            }
        }


        private NumericalMatrix verticalMatrix = new NumericalMatrix();
        private NumericalMatrix horizontalMatrix = new NumericalMatrix();
        private Object locket = new Object();
    }
}
