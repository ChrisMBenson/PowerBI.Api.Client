﻿using System;
using System.Linq;
using System.Reflection;
using PowerBI.Api.Client;
using System.Collections.Generic;
using PowerBI.Api.Client.Configuration;

namespace ClassicClient
{
	class MainClass
	{
		static readonly string DatasetName = "MyProductCatalog";

		public static void Main()
		{
			Console.WriteLine("PowerBI api client test client");
			Console.WriteLine("Version : {0}", Assembly.GetExecutingAssembly().GetName().Version);
			Console.WriteLine();

			try
			{
				PowerBIClient.Initialize("https://api.powerbi.com/v1.0/myorg/dataset/", 
										 "https://login.windows.net/common/oauth2/authorize", 
										 "https://analysis.windows.net/powerbi/api", 
										 "e24d4bbb-1c0a-4324-a0f8-08683084a3e0", 
										 "test@DOMTechDays.onmicrosoft.com", 
										 "Spo060492");

				PowerBIClient.Do(api => {

					var datasets = api.GetDatasets(); 

					var isCreated = api.CreateDataset(DatasetName, true, typeof(Product));

					if(isCreated)
					{
						Console.WriteLine("Dataset created");

						Console.WriteLine("Dataset avaible : {0}", datasets.Select(x => x.Name).Aggregate((current, next) => current + ", " + next));
						var datasetId = api.GetDatasetId(DatasetName);
						Console.WriteLine("Dataset Identifier for MyProductCatalog : {0}", datasetId);
						var isDatasetExist = api.IsDatasetExist(DatasetName);
						Console.WriteLine("Dataset MyProductCatalog exists : {0}", isDatasetExist);
						var isDatasetIdExist = api.IsDatasetIdExist(datasetId);
						Console.WriteLine("Dataset MyProductCatalog Identifier exists : {0}", isDatasetIdExist);
						var isObjectInsert = api.Insert(datasetId, new Product
						{
							CreationDate = DateTime.Now,
							Id = 1,
							IsAvaible = true,
							Name = "Computer",
							Price = 500.00
						});
						Console.WriteLine("Dataset Product insersion : {0}", isObjectInsert);
						var isListInsert = api.InsertAll(datasetId, new List<object>
						{
							new Product
							{
								CreationDate = DateTime.Now,
								Id = 2,
								IsAvaible = true,
								Name = "Screen",
								Price = 120.00
							},
							new Product
							{
								CreationDate = DateTime.Now,
								Id = 3,
								IsAvaible = false,
								Name = "External HDD",
								Price = 75.00
							}
						});

						Console.WriteLine("Dataset Product list insersion : {0}", isListInsert);
						var isDelete = api.Delete<Product>(datasetId);
						Console.WriteLine("Dataset Product rows deleted : {0}", isDelete);
					}
					else
					{
						Console.WriteLine("Failed to create the dataset");
					}
				});
			}
			catch(Exception e)
			{
				Console.WriteLine("{0} => {1} :\n{2}", e.Source, e.Message, e.StackTrace);
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}
	}
}
