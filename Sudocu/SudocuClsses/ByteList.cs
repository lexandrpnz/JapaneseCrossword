using System;
using System.Collections.Generic;
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
                lock (locked)
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
                lock (locked)
                {
                    return _List.ToArray();
                }
            }
        }

        public void Add(byte nValue)
        { _List.Add(nValue); }

        public Int32 GetCount()
        {
            lock (locked)
            {
                return _List.Count;
            }
        }

        public void Clear()
        { 
            lock (locked)
            {
                _List.Clear();
            }
        }


        private List<byte> _List = new List<byte>();
        private Object locked = new Object();
    }
}
