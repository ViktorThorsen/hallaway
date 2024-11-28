create table "Party"
(
    id           integer generated always as identity
        constraint id
            primary key,
    organizer_id integer
        constraint organizer
            references "Person"
);

alter table "Party"
    owner to postgres;

