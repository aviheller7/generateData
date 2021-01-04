using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace generateDataFile
{
	static public class Generator
	{
		
		private static string[] supportedAttributes = new string[] {"ID", "FirstName", "LastName","Gender","Adress" };
		public static string[] SupportedAttributes
		{
			get { return supportedAttributes; }
		}

		#region data access to xml files


		static Stack<string> staticLastNames;
		static string LastNamesPath = @"data\lastNames.xml";
		static XElement LastNamesRoot;


		static Stack<string> staticAdresses;
		static string adressesPath = @"data\adresses.xml";
		static XElement adressRoot;

		static Stack<string> staticMenNames;
		static string MenNamesPath = @"data\menNames.xml";
		static XElement MenNamesRoot;


		static Stack<string> staticWomenNames;
		static string WomenNamesPath =@"data\womenNames.xml";
		static XElement WomenNamesRoot;


		static Stack<string> staticMaleImgs;
		static string MaleImgsPath = @"images\maleHeadshots";

		static Stack<string> staticWomenImgs;
		static string WomennImgsPath = @"images\womenHeadshots";

		#endregion

		#region generating attributes functions
		static private string generateID()
		{
			return Guid.NewGuid().GetHashCode().ToString();
		}
		static public string genAdress()
		{
			if (staticAdresses == null)
			{
				//loading adresses to list
				staticAdresses = new Stack<string>();
				if (!File.Exists(adressesPath))
					return "Not Found any";
				adressRoot = XElement.Load(adressesPath);
				foreach (XElement x in adressRoot.Elements())
				{
					staticAdresses.Push(x.Value);
				}
				Console.ReadKey();
			}
			return staticAdresses.Pop();
		}
		static public string genManName()
		{
			if (staticMenNames == null)
			{
				//loading adresses to list
				staticMenNames = new Stack<string>();
				if (!File.Exists(MenNamesPath))
					return "";
				MenNamesRoot = XElement.Load(MenNamesPath);
				foreach (XElement x in MenNamesRoot.Elements())
					staticMenNames.Push(x.Value);
			}
			return staticMenNames.Pop();
		}
		static public string genWomanName()
		{
			if (staticWomenNames == null)
			{
				//loading adresses to list
				staticWomenNames = new Stack<string>();
				if (!File.Exists(WomenNamesPath))
					return "";
				MenNamesRoot = XElement.Load(WomenNamesPath);
				foreach (XElement x in MenNamesRoot.Elements())
					staticWomenNames.Push(x.Value);
			}
			return staticWomenNames.Pop();
		}
		static public string genLastName()
		{
			if (staticLastNames == null)
			{
				//loading adresses to list
				staticLastNames = new Stack<string>();
				if (!File.Exists(LastNamesPath))
					return "";
				MenNamesRoot = XElement.Load(LastNamesPath);
				foreach (XElement x in MenNamesRoot.Elements())
				{
					staticLastNames.Push(x.Value);
				}
			}
			return staticLastNames.Pop(); 
		}
		static public string genMamImg()
		{
			return "fdsf";
		}
		static public string genWomanImg()
		{
			return "genWomenImgs";
		}
		#endregion

		#region general generators
		static public XElement generateNode(string[] objAndAttributes)
		{
			Dictionary<string, string> objDictionary = new	Dictionary<string, string>();
			var rand = new Random();
			objDictionary.Add("objName", objAndAttributes[0]);
			foreach (string field in objAndAttributes)
			{
				switch (field)
				{
					case "ID":
						string id = generateID();
						objDictionary.Add("ID", id);
						break;
					case "FirstName":
						string firstName;
						if (rand.Next(0,10) > 5){
							firstName = genManName();
							objDictionary.Add("Gender", "Male");
							objDictionary.Add("FirstName", firstName);
						}
						else{
							firstName = genWomanName();
							objDictionary.Add("Gender", "Woman");
							objDictionary.Add("FirstName", firstName);
						}
						break;
					case "LastName":
						string lastName = genLastName();
						objDictionary.Add("LastName", lastName);
						break;
					case "Adress":
						string adress = genAdress();
						objDictionary.Add("Adress", adress);
						break;
					case "Image":
						string Img = genMamImg();
						Console.WriteLine("Img: " + Img);
						break;
					default:
						break;
				}
			}

			XElement element = xmlSerilizer(objDictionary);
			return element;

		}
		static public XmlDocument generatDoc(string[] objAndAttributes, int times)
		{
			if(objAndAttributes == null)
				throw new ArgumentException("objAndAttributes can't be null!");
			string rootName = "";
			if (objAndAttributes[0][objAndAttributes[0].Length - 1] == 's')
				rootName = objAndAttributes + "es";
			else if (objAndAttributes[0][objAndAttributes[0].Length - 1] == 'y')
				rootName = objAndAttributes[0] + "ies";
			else
				rootName = objAndAttributes[0] + "s";
			
			XElement rootElements = new XElement(rootName);
			for(int i=0;i<times;i++){
				XElement element = generateNode(objAndAttributes);
				rootElements.Add(element);
			}
			Console.ReadKey();
			XmlDocument document = new XmlDocument();
			document.LoadXml(rootElements.ToString());
			return document;

		}
		#endregion
		static public XElement xmlSerilizer(Dictionary<string,string> objDictionary)
		{
			XElement xElement, xAttribute;
			xElement = new XElement(objDictionary["objName"]);
			try
			{
				foreach (KeyValuePair<string, string> kvp in objDictionary)
					if (kvp.Key != "objName")
					{
						xAttribute = new XElement(kvp.Key, kvp.Value);
						xElement.Add(xAttribute);
					}
			}
			catch (Exception e)
			{
				throw new Exception("problem with seriliesetion: ", e);
			}
			return xElement;
		}
	}
}
