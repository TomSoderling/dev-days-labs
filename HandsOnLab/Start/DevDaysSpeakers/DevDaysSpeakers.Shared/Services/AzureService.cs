﻿using DevDaysSpeakers.Services;
using DevDaysSpeakers.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;

[assembly: Dependency(typeof(AzureService))]
namespace DevDaysSpeakers.Services
{
    public class AzureService
    {
        public MobileServiceClient Client { get; set; } = null;
        IMobileServiceSyncTable<Speaker> table;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            // TODO: 32.) address for Azure back end
            var appUrl = "https://[END_POINT_NAME_HERE].azurewebsites.net";

            //Create our client
            Client = new MobileServiceClient(appUrl);

            //InitialzeDatabase for path
            var path = "syncstore.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);


            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<Speaker>();

            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            table = Client.GetSyncTable<Speaker>();
        }


        public async Task<IEnumerable<Speaker>> GetSpeakers()
        {
            throw new NotImplementedException("this will be implemented later when we hook to Azure service");

            // TODO: 33.) get speakers from SQLite database table
            //await Initialize();
            //await SyncSpeakers();
            //return await table.OrderBy(s => s.Name).ToEnumerableAsync();
        }

      
        public async Task SyncSpeakers()
        {
            try
            {
                // TODO: 34.) push then pull
                //await Client.SyncContext.PushAsync();
                //await table.PullAsync("allSpeakers", table.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync speakers, that is alright as we have offline capabilities: " + ex);
            }
        }
    }
}