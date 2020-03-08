﻿using System;

namespace Skimur.Backend.Cassandra.Migrations
{
    public class MigrationResourceFinder : IMigrationResourceFinder
    {
        public MigrationResources Find()
        {
            var resources = new MigrationResources();

            var ensureLoaded = Type.GetType("Skimur.Tasks.Migrations.Empty, Skimur.Tasks");

            if (ensureLoaded == null)
            {
                throw new Exception("Couldn't find the Migrations.dll");
            }

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(Migration).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        resources.Add(Activator.CreateInstance(type) as Migration);
                    }
                }
            }

            return resources;
        }
    }
}
