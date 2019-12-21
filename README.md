# RatesStore

RatesStore is the REST service that allows you to receive current exchange rates from its own repository.
It caches rates values every n minutes and keeps a requests history.
The source of exchange rates is https://openexchangerates.org. The resource has conditions for free use.


## Usage
Rest service contains only two GET methods:
```
rates/{from}
rates/{from}/{to}
```

The purchased currency {to} may be absent. In this case the service returns all possible currencies
relative to selling currency {from}.
If {from} or {to} arguments are incorrect, service returns BadRequest with ArgumentException message

The service return RatesResponse json object. See the classes below.
The returning status code can be BadRequest and InternalServerError.

```C#
public class RatesResponse
    {
        public string From { get; set; }
        public RateInfo[] Rates { get; set; }
    }
public struct RateInfo
    {
        public string To { get; set; }
        public decimal Rate { get; set; }
        public DateTime ExpireAt { get; set; }
    }
```


## Database
The application uses MSSQL as database store. The connection is implemented using by Entity Framework 6 Code First via UnitOfWork pattern.
```C#
public class UnitOfWork : IDisposable
    {
        private readonly RatesDbContext db;
        private Hashtable repositories;
        private bool disposed = false;

        public UnitOfWork()
        {
            db = new RatesDbContext();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (repositories == null)
                repositories = new Hashtable();

            var type = typeof(TEntity).Name;
            if (repositories.ContainsKey(type)) return (IRepository<TEntity>)repositories[type];

            var repositoryType = typeof(Repository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(TEntity)), db);

            repositories.Add(type, repositoryInstance);
            return (IRepository<TEntity>)repositories[type];
        }
```


It is recommended to run Init console application from the solution for filling database by rates and rates pairs. It will increase the application performance since RateRelation tables uses non-clustered indexes and MSSQL server won't reacreate B-tree with every new rate request. 

Main updates(rates pairs values) and inserts(logs and request history) are executing asynchronously to increase performance of the application.

## Configuration file
The configuration file allows us to configure database connection string and the other points:
```config
<add key="ApiUrl" value="http://openexchangerates.org/api" />
<add key="ApiMethod" value="latest.json" />
<add key="ApiKey" value="" />
<add key="RateRoundDecimals" value="4" />
<add key="ValidPeriodMinutes" value="30" />
```

## Tests
The solution has some loading and returning value tests.

## Logger
The database logger first fills the buffer, and then, after filling the buffer, it writes logs to the database asynchronously. 

```C#
public class DbLogger
    {
        static readonly int countToFlush = 14;
        private static readonly ConcurrentBag<Log> concurrentBuffer = new ConcurrentBag<Log>();

        //save current logs to db and clear buffer
        private static void FlushBufferAsync()
        {
            var arrayOfLofs = concurrentBuffer.ToArray();

            //clear
            lock (concurrentBuffer)
            {
                Log someItem;
                while (!concurrentBuffer.IsEmpty)
                {
                    concurrentBuffer.TryTake(out someItem);
                }
            }

            Task.Run(() =>
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    unitOfWork.Repository<Log>().AddRange(arrayOfLofs);
                    unitOfWork.Save();
                }
            });
        }
