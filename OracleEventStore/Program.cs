using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.DataAccess.Client;
using OracleEventStore.Domain;
using OracleEventStore.Utils;

namespace OracleEventStore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sqlConnection = new OracleConnection(ConfigurationManager.AppSettings["OracleConnection"]))
            {
                //// x2 events
                //var eventsToBeInserted = new List<Event>
                //{
                //        new Event()
                //        {
                //            Data = ProtoSerializer.Serialize("event1"),
                //            Date = DateTime.Now
                //        },
                //        new Event()
                //        {
                //            Data = ProtoSerializer.Serialize("event2"),
                //            Date = DateTime.Now
                //        }
                //};

                // x10 events
                var eventsToBeInserted = new List<Event>
                {
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event1"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event2"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event3"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event4"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event5"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event6"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event7"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event8"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event9"),
                            Date = DateTime.Now
                        },
                        new Event()
                        {
                            Data = ProtoSerializer.Serialize("event10"),
                            Date = DateTime.Now
                        }
                };

                sqlConnection.Open();

                for (int i = 0; i < 10; ++i)
                {
                    int expectedVersion = 1; //i * 10 + 1;
                    Guid aggregateId = Guid.NewGuid(); //new Guid("0eaf40de-6481-4895-a654-e6bea1c6a594");

                    foreach (var e in eventsToBeInserted)
                        e.AggregateId = aggregateId.ToString();

                    using (var cmd = new OracleCommand("SYSTEM.SAVEEVENTS", sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        PrepareParameters(cmd, aggregateId, expectedVersion,
                            eventsToBeInserted);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            Console.WriteLine("Work done.");
        }

        private static void PrepareParameters(OracleCommand command, Guid aggregateId, int expectedVersion, List<Event> events)
        {
            var parameter = new OracleParameter()
            {
                OracleDbType = OracleDbType.Varchar2,
                Direction = ParameterDirection.Input,
                ParameterName = "AggregateId",
                Value = aggregateId.ToString()
            };
            command.Parameters.Add(parameter);

            parameter = new OracleParameter()
            {
                OracleDbType = OracleDbType.Int32,
                Direction = ParameterDirection.Input,
                ParameterName = "ExpectedVersion",
                Value = expectedVersion
            };
            command.Parameters.Add(parameter);

            parameter = new OracleParameter()
            {
                OracleDbType = OracleDbType.Array,
                Direction = ParameterDirection.Input,
                ParameterName = "Eventss",
                UdtTypeName = "SYSTEM.EVENTSTYPE",
                Value = new Events
                {
                    Value = events.ToArray()
                }
            };
            command.Parameters.Add(parameter);

            command.BindByName = true;
        }
    }
}
