using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SudocuClsses
{

    public abstract class IMath
    {
        public abstract Int64 GetVar(Int32 ObjectCount, Int32 CellCount);
    }


    public class Math: IMath
    {
        public override Int64 GetVar(Int32 ObjectCount, Int32 CellCount)
        {
            return _FactorialEx(
                ObjectCount + CellCount, ObjectCount > CellCount ? ObjectCount : CellCount)
                / _FactorialEx(ObjectCount < CellCount ? ObjectCount : CellCount, 1);
        }


        private Int64 _FactorialEx(Int32 Begin, Int32 End)
        {
            if (Begin > End)
            {
                return Begin * _FactorialEx(Begin - 1, End);
            }
            return 1;
        }
    }

    [Serializable()]
     public class CachedMath : Math
     {
        [XmlElement]
        public XmlSerializableDictionary<Int32, XmlSerializableDictionary<Int32, Int64>> _GetVarCahe
        {get;set;}

        public CachedMath()
        {
            _GetVarCahe = new XmlSerializableDictionary<Int32, XmlSerializableDictionary<Int32, Int64>>();
        }

        public override Int64 GetVar(Int32 ObjectCount, Int32 CellCount)
        {
            try
            {
                return _GetVarCahe[ObjectCount][CellCount];
            }
            catch
            {
                if (!_GetVarCahe.ContainsKey(ObjectCount))
                {
                    _GetVarCahe.Add(ObjectCount, new XmlSerializableDictionary<Int32, Int64>());
                }
                _GetVarCahe[ObjectCount][CellCount] = base.GetVar(ObjectCount, CellCount);
            }
            return _GetVarCahe[ObjectCount][CellCount];
        }
        public static CachedMath Load(String Path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CachedMath));
            using (Stream stream = new FileStream(Path, FileMode.Open))
            {
                return (CachedMath)serializer.Deserialize(stream);
            }
        }

        public void SaveCache(string path)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(CachedMath));
            Stream stream = new FileStream(path, FileMode.Create);

            Serializer.Serialize(stream, this, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) }));
            stream.Close();
        }

     }

}
