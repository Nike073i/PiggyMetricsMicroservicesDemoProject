set -ue

let "timeout = $(date +%s) + 15"

echo "==> Waiting for Consul"
while ! curl -f -s http://localhost:8500/v1/status/leader | grep "[0-9]:[0-9]"; do
  if [ $(date +%s) -gt $timeout ]; then echo "==> Consul timeout"; exit 1; fi
  sleep 1
  echo "==> Waiting for Consul"
done

echo "==> Load configuration"
cd $CONSUL_KV_INIT_DIR

for json_file in $CONSUL_KV_INIT_DIR/**/*.json; do
  key=$(basename "$json_file")
  subprefix=$(basename $(dirname "$json_file"))
  echo "==> Loading $key from $subprefix"
  consul kv put "$subprefix/$key" @$json_file
done