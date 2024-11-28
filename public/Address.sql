create table "Address"
(
    location_id integer generated always as identity
        constraint address_pk
            primary key,
    city        varchar,
    street      varchar
);

alter table "Address"
    owner to postgres;

