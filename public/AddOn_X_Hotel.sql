create table "AddOn_X_Hotel"
(
    addon_id integer not null
        constraint addon_x_hotel_addon_addon_id_fk
            references "AddOn",
    hotel_id integer not null
        constraint addon_x_hotel_hotel_hotel_id_fk
            references "Hotel",
    constraint addon_x_hotel_pk
        primary key (hotel_id, addon_id)
);

alter table "AddOn_X_Hotel"
    owner to postgres;

