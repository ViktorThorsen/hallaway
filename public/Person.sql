create table "Person"
(
    user_id       integer generated always as identity
        constraint person_pk
            primary key,
    name          varchar,
    phone         varchar,
    email         varchar,
    date_of_birth date,
    party_id      integer

);

alter table "Person"
    owner to postgres;

