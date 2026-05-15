################################################## functions ##################################################

generate_keys() {
  KEY_DIR="tmp"
  PRIVATE_KEY="$KEY_DIR/private.key"
  PUBLIC_KEY="$KEY_DIR/public.key"

  if [ ! -d "$KEY_DIR" ]; then
    mkdir -p "$KEY_DIR"
  fi
  openssl genpkey -algorithm RSA -out $PRIVATE_KEY -pkeyopt rsa_keygen_bits:2048 # Generating RSA keys
  openssl rsa -in $PRIVATE_KEY -pubout -out $PUBLIC_KEY # Extracting public key
}

################################################## execute ##################################################

generate_keys
