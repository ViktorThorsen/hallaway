create table admin
(
    admin_id      integer generated always as identity
        constraint admin_pk
            primary key,
    name          varchar,
    phone         varchar,
    email         varchar,
    date_of_birth date
);

alter table admin
    owner to postgres;

