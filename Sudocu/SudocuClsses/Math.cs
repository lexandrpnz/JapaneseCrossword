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
        public abstract Byte[] CalcPositions(Byte ObjCount, Byte FreeCellCount, Int64 Var);
    }


    public class Math: IMath
    {
        public override Byte[] CalcPositions(Byte ObjCount, Byte FreeCellCount, Int64 Var)
        {
            Byte[] Positions = new Byte[ObjCount];

            if (0 == Var)
            {
                return Positions;
            }
            if (Var > GetVar(ObjCount, FreeCellCount))
            {
                return Positions;
            }

            Int64 tmpVar = Var;
            Byte Summposition = 0;
            for (Byte i = 1; i < ObjCount; i++)
            {
                do
                {
                    if (tmpVar > GetVar(ObjCount - i, FreeCellCount - Summposition))
                    {
                        tmpVar -= GetVar(ObjCount - i, FreeCellCount - Summposition);
                        Positions[i - 1]++;
                        Summposition++;
                    }
                    else
                    {
                        break;
                    }
                } while (true);
            }

            Positions[ObjCount - 1] = (Byte)(tmpVar - 1);
            return Positions;
        }


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
        public XmlSerializableDictionary<Int32, XmlSerializableDictionary<Int32, Int64>> _GetVarCache
        {get;set;}

        public CachedMath()
        {
            _GetVarCache = new XmlSerializableDictionary<Int32, XmlSerializableDictionary<Int32, Int64>>();
        }

        public override Int64 GetVar(Int32 ObjectCount, Int32 CellCount)
        {
            try
            {
                return _GetVarCache[ObjectCount][CellCount];
            }
            catch
            {
                if (!_GetVarCache.ContainsKey(ObjectCount))
                {
                    _GetVarCache.Add(ObjectCount, new XmlSerializableDictionary<Int32, Int64>());
                }
                _GetVarCache[ObjectCount][CellCount] = base.GetVar(ObjectCount, CellCount);
            }
            return _GetVarCache[ObjectCount][CellCount];
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
