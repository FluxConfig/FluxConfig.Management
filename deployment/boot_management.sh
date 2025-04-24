#!/bin/bash

help_description()
{
  echo "Example of usage :"
  echo "./boot_management.sh -c <path_to_config_file>"
  echo ""
  echo "Arguments description :"
  echo "<path_to_config_file> - path to FluxConfig management .cfg file"
  echo "To learn about it's structure visit https://github.com/FluxConfig/FluxConfig.Management/blob/master/deployment/DEPLOY.md"
}

boot_script()
{
  # Parse CLI Arguments
  ########
  while getopts "c:h" opt
    do
      case "$opt" in
        c ) local pathToConfig="$OPTARG" ;;
        h ) 
          help_description
          exit 0  ;;
        ? ) 
          help_description
          exit 1  ;;
      esac
  done
    
  if [ -z "$pathToConfig" ]; then 
    echo "Missing -c CLI argument."
    help_description
    exit 1
  fi
  ########
  
  # Check docker installation
  ########
  if ! command -v docker 2>&1 >/dev/null
    then
        echo "Docker could not be found. Install Docker first."
        exit 1
  fi
    
  if ! command -v docker-compose 2>&1 >/dev/null
    then
        echo "Docker-compose could not be found. Install Docker-compose first."
        exit 1
  fi
  ########
  
  # Fetching compose file
  ########
  local COMPOSE_URL="https://raw.githubusercontent.com/FluxConfig/FluxConfig.Management/refs/heads/master/deployment/docker-compose.yml"
  echo "Fetching docker-compose.yml..."
  
  curl -fsSL -o docker-compose.yml "$COMPOSE_URL"
  
  if [ $? -ne 0 ]; then
    echo "Failed to fetch docker-compose.yml:"
    echo "  - HTTP Error or network failure"
    exit 1
  elif [ ! -f "docker-compose.yml" ]; then
    echo "Download failed - no file created"
    exit 1
  elif [ ! -s "docker-compose.yml" ]; then
    echo "Download failed - empty file received"
    exit 1
  fi
  
  echo "docker-compose.yml successfully downloaded"
  echo ""
  ########
  
  
  # Check config file existence and download if needed
  ########
  if [ ! -f "$pathToConfig" ]
  then
    local downloadAc
    echo "Config file not found. Do you want to download template file? y/n"
    read downloadAc
    if [ "$downloadAc" == "y" ]; then
      local TEMPLATE_URL="https://raw.githubusercontent.com/FluxConfig/FluxConfig.Management/refs/heads/master/deployment/management.template.cfg"
      echo ""
      echo "Fetching management.template.cfg..."
      
      curl -fsSL -o management.template.cfg "$TEMPLATE_URL"
        
      if [ $? -ne 0 ]; then
        echo "Failed to fetch docker-compose.yml:"
        echo "  - HTTP Error or network failure"
        exit 1
      elif [ ! -f "docker-compose.yml" ]; then
        echo "Download failed - no file created"
        exit 1
      elif [ ! -s "docker-compose.yml" ]; then
        echo "Download failed - empty file received"
        exit 1
      fi
      
      echo "management.template.cfg successfully downloaded"
      echo "Please fill the configuration file and run this script again."
      echo ""
      exit 0
    fi
    exit 1;
  fi
  ########
  
  # Load cfg variables to .env
  ########
  if [ ! -f ".env" ]
  then
    touch ".env"
  fi
  truncate -s 0 ".env"
  
  echo "Reading .cfg file..."
  while IFS= read -r line || [[ -n "$line" ]]; do
      if [[ "$line" =~ ^#.*$ || -z "$line" ]]; then
          continue
      fi
  
      key=$(echo "$line" | cut -d '=' -f 1)
      value=$(echo "$line" | cut -d '=' -f 2-)
  
      key=$(echo "$key" | xargs)
      value=$(echo "$value" | xargs)
      
      if [ "$key" == "PG_USER" ] && [ -z "$value" ]; then
        echo ""
        echo "Generating value for PG_USER"
        value="u$(uuidgen | tr -d '-' | tr '[:upper:]' '[:lower:]')"
      fi

      if [ "$key" == "PG_PSWD" ] && [ -z "$value" ]; then
        echo ""
        echo "Generating value for PG_PSWD"
        value=$(uuidgen | tr -d '-' | tr '[:upper:]' '[:lower:]')
      fi

      if [ "$key" == "FC_API_KEY" ] && [ -z "$value" ]; then
        echo ""
        echo "Generating value for FC_API_KEY"
        value=$(uuidgen | tr -d '-' | tr '[:upper:]' '[:lower:]')
      fi
    
      printf "$key=$value\n" >> .env
      echo "Loaded: $key=$value"
  done < "$pathToConfig"
  ########
  
  # Generating pg credentials for internal usage
  ########
  postgres_users_vars=("PG_FCM_APP_USER" "PG_MIGRATION_USER")
  pguv_descriptions=("FCM Application User" "FCM Application Password" "Migration User" "Migration Password")
  postgres_pswd_vars=("PG_FCM_APP_PSWD" "PG_MIGRATION_PSWD")
  pgpswd_descriptions=("FCM Application Password" "Migration Password")
  
  # users
  echo ""
  for i in "${!postgres_users_vars[@]}"; do
    var="${postgres_users_vars[$i]}"
    desc="${pguv_descriptions[$i]}"
    if ! grep -q "^$var=" .env; then
      echo "Generating $desc"
      value="u$(uuidgen | tr -d '-' | tr '[:upper:]' '[:lower:]')"
      echo "$var=$value" >> .env
    fi
  done
  
  #passwords
  for i in "${!postgres_pswd_vars[@]}"; do
      var="${postgres_pswd_vars[$i]}"
      desc="${pgpswd_descriptions[$i]}"
      if ! grep -q "^$var=" .env; then
        echo "Generating $desc"
        value=$(uuidgen | tr -d '-' | tr '[:upper:]' '[:lower:]')
        echo "$var=$value" >> .env
      fi
    done
  ########
  
  # Booting
  ########
  echo ""
  echo "Starting FluxConfig Management..."
  docker-compose up -d
  exit 0
  ########
}


boot_script "$@"