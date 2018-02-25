using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JCClasses
{

    public abstract class IMath
    {
        public abstract Int64 GetVar(Int32 ObjectCount, Int32 CellCount);
        public abstract Byte[] CalcPositions(Byte ObjCount, Byte FreeCellCount, Int64 Var);
    }


    public class Math : IMath
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

    public class MatchCach: XmlSerializableDictionary<Int32, XmlSerializableDictionary<Int32, Int64>>
    {}

    [Serializable()]
    public class CachedMath : Math
     {
        [XmlElement]
        public MatchCach _GetVarCache
        {get;set;}

        private object locObj = new object();
        private const string filename = "MatchCach.xml";

        ~CachedMath()
        {
            Save(filename);
        }
        public CachedMath()
        {
            try
            {
                Load(filename);
            }
            catch (FileNotFoundException)
            {
                _GetVarCache = new MatchCach();
            }
        }

        public override Int64 GetVar(Int32 ObjectCount, Int32 CellCount)
        {
            lock(locObj)
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
        }
        public void Load(String Path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MatchCach));
            using (Stream stream = new FileStream(Path, FileMode.Open))
            {
                this._GetVarCache = (serializer.Deserialize(stream) as MatchCach);
            }
        }

        public void Save(string path)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(MatchCach));
            Stream stream = new FileStream(path, FileMode.Create);

            Serializer.Serialize(stream, this._GetVarCache, new XmlSerializerNamespaces(new XmlQualifiedName[] { new XmlQualifiedName(string.Empty) }));
            stream.Close();
        }

     }

}
