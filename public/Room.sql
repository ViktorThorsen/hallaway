create table "Room"
(
    room_id  integer generated always as identity
        constraint room_pk
            primary key,
    price    double precision,
    size     integer,
    hotel_id integer
);

alter table "Room"
    owner to postgres;

