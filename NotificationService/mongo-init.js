db = db.getSiblingDB('admin');

db.auth('root', 'example');

db = db.getSiblingDB('notification_db');

db.recipients.insertOne({});

db.createUser({
    user: 'root',
    pwd: 'example',
    roles: [
        {
            role: 'dbAdmin',
            db: 'notification_db',
        },
        {
            role: 'readWrite',
            db: 'notification_db',
        }
    ]
});

db.recipients.remove({});