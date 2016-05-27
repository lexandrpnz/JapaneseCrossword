using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SudocuClsses
{
    /**
     * Числовая матрица
     * отражающая исходные данные сканворда
     * по вертикали и по горизонтали
     */
    [Serializable()]
    [XmlRoot("Matrix")]
    public class NumericalMatrix:  List<ByteList>
    {
        public NumericalMatrix()
        { }

        ~NumericalMatrix()
        { }

        public Int32 GetMaxCount()
        {
            lock (_Locked)
            {
                int i;
                Int32 ret = 1;
                for (i = 0; i < Count; i++)
                {
                    if (this[i].GetCount() > ret)
                        ret = this[i].GetCount();
                }
                return ret;
            }
        }
        private Object _Locked = new Object();
    }
}
