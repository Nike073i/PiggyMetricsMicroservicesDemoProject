db = db.getSiblingDB('admin');

db.auth('root', 'example');

db = db.getSiblingDB('account_db');

db.accounts.insertOne({});

db.createUser({
    user: 'root',
    pwd: 'example',
    roles: [
        {
            role: 'dbAdmin',
            db: 'account_db',
        },
        {
            role: 'readWrite',
            db: 'account_db',
        }
    ]
});

db.accounts.remove({});