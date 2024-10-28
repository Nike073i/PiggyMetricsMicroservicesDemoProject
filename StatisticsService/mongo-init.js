db = db.getSiblingDB('admin');

db.auth('root', 'example');

db = db.getSiblingDB('statistics_db');

db.datapoints.insertOne({});

db.createUser({
    user: 'root',
    pwd: 'example',
    roles: [
        {
            role: 'dbAdmin',
            db: 'statistics_db',
        },
        {
            role: 'readWrite',
            db: 'statistics_db',
        }
    ]
});

db.datapoints.remove({});