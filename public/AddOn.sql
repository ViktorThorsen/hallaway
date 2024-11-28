create table "AddOn"
(
    addon_id    integer generated always as identity
        constraint addon_pk
            primary key,
    name        varchar,
    description varchar,
    price       double precision,
    hotel       integer
        constraint addon_hotel_hotel_id_fk
            references "Hotel",
    "order"     integer
        constraint addon_order_order_id_fk
            references "Order"
);

alter table "AddOn"
    owner to postgres;

