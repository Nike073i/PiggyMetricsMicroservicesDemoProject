FROM consul:1.15.4

ENV CONSUL_KV_INIT_DIR=/var/local/consul-init.d
RUN mkdir -p $CONSUL_KV_INIT_DIR

COPY ./Consul/consul-init.sh /usr/local/bin/

RUN sed -ie 's|^\(set .*\)|\1\n/usr/local/bin/consul-init.sh \&|' /usr/local/bin/docker-entrypoint.sh