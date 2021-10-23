create sequence public."Entity_entity_id_seq"
    as integer;

alter sequence public."Entity_entity_id_seq" owner to postgres;

create sequence public."Menu_product_id_seq"
    as integer;

alter sequence public."Menu_product_id_seq" owner to postgres;

create sequence public."Sizes_size_id_seq"
    as integer;

alter sequence public."Sizes_size_id_seq" owner to postgres;

create sequence public."Types_type_id_seq"
    as integer;

alter sequence public."Types_type_id_seq" owner to postgres;

create sequence public."Users_user_id_seq"
    as integer;

alter sequence public."Users_user_id_seq" owner to postgres;

create sequence public."Orders_order_id_seq"
    as integer;

alter sequence public."Orders_order_id_seq" owner to postgres;

create table if not exists public.entity
(
    entity_id integer default nextval('"Entity_entity_id_seq"'::regclass) not null
        constraint entity_pk
            primary key,
    name      text                                                        not null
);

alter table public.entity
    owner to postgres;

alter sequence public."Entity_entity_id_seq" owned by public.entity.entity_id;

create unique index if not exists entity_entity_id_uindex
    on public.entity (entity_id);

create unique index if not exists entity_name_uindex
    on public.entity (name);

create table if not exists public.sizes
(
    size_id integer default nextval('"Sizes_size_id_seq"'::regclass) not null
        constraint sizes_pk
            primary key,
    name    text
);

alter table public.sizes
    owner to postgres;

alter sequence public."Sizes_size_id_seq" owned by public.sizes.size_id;

create unique index if not exists sizes_size_id_uindex
    on public.sizes (size_id);

create table if not exists public.types
(
    type_id integer default nextval('"Types_type_id_seq"'::regclass) not null
        constraint types_pk
            primary key,
    name    text                                                     not null
);

alter table public.types
    owner to postgres;

alter sequence public."Types_type_id_seq" owned by public.types.type_id;

create table if not exists public.menu
(
    product_id   integer default nextval('"Menu_product_id_seq"'::regclass) not null
        constraint menu_pk
            primary key,
    product_type integer                                                    not null
        constraint menu_types_type_id_fk
            references public.types,
    price        integer                                                    not null,
    name         text                                                       not null,
    size         integer                                                    not null
        constraint menu_sizes_size_id_fk
            references public.sizes,
    available    boolean
);

alter table public.menu
    owner to postgres;

alter sequence public."Menu_product_id_seq" owned by public.menu.product_id;

create unique index if not exists menu_product_id_uindex
    on public.menu (product_id);

create unique index if not exists types_type_id_uindex
    on public.types (type_id);

create table if not exists public.users
(
    user_id     integer default nextval('"Users_user_id_seq"'::regclass) not null
        constraint users_pk
            primary key,
    firstname   text                                                     not null,
    lastname    text                                                     not null,
    telephone   text                                                     not null,
    address     text                                                     not null,
    city        text                                                     not null,
    postal_code text                                                     not null,
    entity_id   integer                                                  not null
        constraint users_entity_entity_id_fk
            references public.entity,
    username    text,
    password    text,
    available   boolean
);

alter table public.users
    owner to postgres;

alter sequence public."Users_user_id_seq" owned by public.users.user_id;

create unique index if not exists users_user_id_uindex
    on public.users (user_id);

create table if not exists public.orders
(
    order_id integer default nextval('"Orders_order_id_seq"'::regclass) not null
        constraint orders_pk
            primary key,
    date     timestamp                                                  not null,
    client   integer                                                    not null
        constraint orders_users_user_id_fk
            references public.users,
    clerk    integer                                                    not null
        constraint orders_users_user_id_fk_2
            references public.users,
    driver   integer                                                    not null
        constraint orders_users_user_id_fk_3
            references public.users,
    product  text                                                       not null,
    product2 text,
    product3 text,
    product4 text,
    product5 text,
    product6 text,
    price    integer                                                    not null,
    paid     boolean default false                                      not null
);

alter table public.orders
    owner to postgres;

alter sequence public."Orders_order_id_seq" owned by public.orders.order_id;

create unique index if not exists orders_order_id_uindex
    on public.orders (order_id);
