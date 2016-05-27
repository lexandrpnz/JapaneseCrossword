using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace SudocuClsses
{
    [Serializable()]
    [XmlRoot("List")]
    public class ByteList
    {

        public ByteList()
        { }

        ~ByteList()
        { }


        [XmlElement("B")]
        public byte[] list
        {
            set
            {
                lock (_Locked)
                {
                    _List.Clear();
                    byte i;
                    for (i = 0; i < value.Length; i++)
                    {
                        _List.Add(value[i]);
                    }
                }
            }
            get
            {
                lock (_Locked)
                {
                    return _List.ToArray();
                }
            }
        }

        public void Add(byte nValue)
        { _List.Add(nValue); }

        public Int32 GetCount()
        {
            lock (_Locked)
            {
                return _List.Count;
            }
        }

        public void Clear()
        { 
            lock (_Locked)
            {
                _List.Clear();
            }
        }


        private List<byte> _List = new List<byte>();
        private Object _Locked = new Object();
    }
}
