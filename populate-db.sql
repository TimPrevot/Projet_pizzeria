INSERT INTO entity VALUES (default, 'clerk');
INSERT INTO entity VALUES (default, 'client');
INSERT INTO entity VALUES (default, 'driver');

INSERT INTO sizes VALUES (default, 'm');
INSERT INTO sizes VALUES (default, 'l');
INSERT INTO sizes VALUES (default, 'xl');

INSERT INTO types VALUES (default, 'pizza');
INSERT INTO types VALUES (default, 'drink');

INSERT INTO menu VALUES (default, 1, 12, 'Margherita', 1, true);
INSERT INTO menu VALUES (default, 1, 14, 'Margherita', 2, true);
INSERT INTO menu VALUES (default, 1, 15, 'Tartufata', 1, true);
INSERT INTO menu VALUES (default, 1, 18, 'Tartufata', 2, true);
INSERT INTO menu VALUES (default, 1, 13, 'Regina', 1, true);
INSERT INTO menu VALUES (default, 1, 16, 'Regina', 2, true);
INSERT INTO menu VALUES (default, 2, 3, 'Coca-cola', 1, true);
INSERT INTO menu VALUES (default, 2, 3, 'Ice-Tea', 1, true);

INSERT INTO users VALUES (default, 'firstclient', 'test', 'test', 'test', 'test', 'test', 2, null, null, null);
INSERT INTO users VALUES (default, 'firstdriver', 'test2', 'test2', 'test2', 'test2', 'test2', 3, null, null, true);
