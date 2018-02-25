using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace JCClasses
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
                lock (lockObj)
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
                lock (lockObj)
                {
                    return _List.ToArray();
                }
            }
        }

        public void Add(byte nValue)
        { _List.Add(nValue); }

        public Int32 GetCount()
        {
            lock (lockObj)
            {
                return _List.Count;
            }
        }

        public void Clear()
        { 
            lock (lockObj)
            {
                _List.Clear();
            }
        }


        private List<byte> _List = new List<byte>();
        private Object lockObj = new Object();
    }
}
