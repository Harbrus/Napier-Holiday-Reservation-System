using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Data
{
    /*
    * Author name: Andrea Silvestro Ortino
    * Data.SingletonStorage is a class which stores permanently a Customerdict object in a bynary file. 
    * Date last modified: 22/11/2017 17:03
    * This class implement the Singleton Design Pattern because only one instance of this object will be required during the running of the application.
    */ 

    public class SingletonStorage
    {
        private const string filename = "data.dat";
        private BinaryFormatter formatter = new BinaryFormatter();

        private CustomerDict _custDict = new CustomerDict();
        private static SingletonStorage _instance = null;

        // SingletonStorage private constructor to avoid the creation of instance of this class without using the Instance property.
        private SingletonStorage() {}

        // Instance property is the only way to create an object of this class. It first checks the existence of an instance of this class (creating one if not already done before),
        // otherwise it will return the current and only instance existing. 
        public static SingletonStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SingletonStorage();
                }
                
                return _instance;
            }
        }

        // Filename property with get method to return the corrispective private attribute.
        public string Filename
        {
            get
            {
                return filename;
            }
        }

        // CustDict property with get/set methods to return and/or set the corrispective private object attribute.
        public CustomerDict CustDict
        {
            get
            {
                return _custDict;
            }
            set
            {
                this._custDict = value;
            }
        }

        // This method create a binary file and serialize a CustomerDict object if the file does not exist already.
        public void serializeDict()
        {
            if (File.Exists(filename))
            {
                throw new ArgumentException("File already exists. You should use 'UpdateFile' instead.");
            }
            else
            {
                FileStream stream = File.Create(filename);
                stream.Position = 0;
                formatter.Serialize(stream, CustDict);
                stream.Position = 0;
                stream.Close();
            }
        }

        // This method retrieve from a binary file a CustomerDict object only if the file exists already.
        public CustomerDict deserializeDict()
        {
            if (File.Exists(filename))
            {
                FileStream stream = File.OpenRead(filename);
                stream.Position = 0;
                CustDict = (CustomerDict)formatter.Deserialize(stream);
                stream.Close();

                return CustDict;
            }
            else
                throw new ArgumentException("File already exists. You should use 'UpdateFile' instead.");
            
        }

        // This method will be called for edit CustomerDict object information. If the file already exist, it will be deleted, then the CustomerDict object will be serialized again.
        public void updateFile()
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            FileStream stream = File.Create(filename);
            stream.Position = 0;
            formatter.Serialize(stream, CustDict);

            stream.Close();
        }
    }
}
