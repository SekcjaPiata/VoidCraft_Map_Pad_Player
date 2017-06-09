using System;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

using System.Xml;
using System.Runtime.Serialization;


namespace Raw_Materials_C
{
    //[DataContractSer]
    [DataContract]
    public class RawMaterials
    {
        internal class OutOfWoodException : Exception
        {
            public OutOfWoodException(string message) : base(message) { }
        }
        internal class OutOfStoneException : Exception
        {
            public OutOfStoneException(string message) : base(message) { }
        }
        internal class OutOfLianasException : Exception
        {
            public OutOfLianasException(string message) : base(message) { }
        }
        internal class OutOfMetalException : Exception
        {
            public OutOfMetalException(string message) : base(message) { }
        }
        internal class OutOfWaterException : Exception
        {
            public OutOfWaterException(string message) : base(message) { }
        }
        internal class OutOfFoodException : Exception
        {
            public OutOfFoodException(string message) : base(message) { }
        }


        
        private int _wood;
        [DataMember]
        public int Wood
        {
            get { return _wood; }
            set {
                if (_wood + value < 0)
                {
                    throw new OutOfWoodException("Zbyt ma³o drewna!");
                } else
                {
                    _wood = value;
                }
            }
        }
        
        private int _stone;
        [DataMember]
        public int Stone
        {
            get { return _stone; }
            set {
                if (_stone + value < 0)
                {
                    throw new OutOfStoneException("Zbyt ma³o kamienia!");
                } else
                {
                    _stone = value;
                }
            }
        }
        
        private int _lianas;
        [DataMember]
        public int Lianas
        {
            get { return _lianas; }
            set {
                if (Lianas + value < 0)
                { throw new OutOfLianasException("Zbyt ma³o lian!"); } else
                {
                    _lianas = value;
                }
            }
        }
        
        private int _metal;
        [DataMember]
        public int Metal
        {
            get { return _metal; }
            set {
                if (_metal + value < 0)
                { throw new OutOfMetalException("Zbyt ma³o metalu!"); } else
                {
                    _metal = value;
                }
            }
        }
       
        private int _water;
        [DataMember]
        public int Water
        {
            get { return _water; }
            set {
                if (_water + value < 0)
                { throw new OutOfWaterException("Zbyt ma³o wody!"); } else
                { _water = value; }
            }
        }
      
        private int _food;
        [DataMember]
        public int Food
        {
            get { return _food; }
            set {
                if (_food + value < 0)
                {
                    throw new OutOfFoodException("Zbyt ma³o jedzienia!");
                } else
                {
                    _food = value;
                }
            }
        }

        public bool Contains(RawMaterials checked_materials)//sprawdza czy dane materia³y zawieraj¹ podane w parametrach
            //na przyk³ad jeœli chcemy sprawdziæ, czy gracz mo¿e sobie pozwoliæ na scrafcenie czegoœ to porównujemy jego materia³y z wymaganymi
        {
            if (this.Wood < checked_materials.Wood)
                return false;
            if (this.Stone < checked_materials.Stone)
                return false;
            if (this.Lianas < checked_materials.Lianas)
                return false;
            if (this.Metal < checked_materials.Metal)
                return false;
            if (this.Water < checked_materials.Water)
                return false;
            if (this.Food < checked_materials.Food)
                return false;
            return true;
            
        }


        //Konstruktory
        public RawMaterials()
        {
            Wood = 0;
            Stone = 0;
            Lianas = 0;
            Metal = 0;
            Water = 0;
            Food = 0;
        }

        public RawMaterials(int Wood, int Stone, int Lianas, int Metal, int Water, int Food)
        {
            this.Wood = Wood;
            this.Stone = Stone;
            this.Lianas = Lianas;
            this.Metal = Metal;
            this.Water = Water;
            this.Food = Food;
        }
        public void SaveToFile(string filename)
        {
            //try
            //{
            //    XmlRootAttribute root_atribute = new XmlRootAttribute();
            //    root_atribute.IsNullable = true;
            //    root_atribute.ElementName = "Dupa";
            //    XmlSerializer xmlFormat = new XmlSerializer(typeof(RawMaterials), root_atribute);
            //    using (FileStream fStream = new FileStream(@"r.xml",FileMode.OpenOrCreate))
            //    {
            //        xmlFormat.Serialize(fStream, this);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}

            //DataContractSerializer dcs = new DataContractSerializer(typeof(RawMaterials));

            

            //using (Stream stream = new FileStream("C://r", FileMode.Create, FileAccess.Write))
            //{
            //    using (XmlDictionaryWriter writer =
            //        XmlDictionaryWriter.CreateTextWriter(stream))
            //    {
            //        writer.WriteStartDocument();
            //        dcs.WriteObject(writer, this);
            //    }
            //}
        }
    }
}