# FluxConfig.Management deployment guidance

## Getting started

### 0. Prerequisites

**Docker and docker-compose installed on the system.**

### 1. Download deployment script

```bash
curl -LJO https://raw.githubusercontent.com/FluxConfig/FluxConfig.Management/refs/heads/master/deployment/boot_management.sh
```

### 2. Fill .cfg file as it shown in [template](https://github.com/FluxConfig/FluxConfig.Management/blob/master/deployment/management.template.cfg)

**You can create and fill it manually**

**Download and fill it manually**

```bash
curl -LJO https://raw.githubusercontent.com/FluxConfig/FluxConfig.Management/refs/heads/master/deployment/management.template.cfg
```

**Or let the script download it.**

```bash
./boot_management.sh -c "non-existing.cfg"
Fetching docker-compose.yml...
docker-compose.yml successfully downloaded

Config file not found. Do you want to download template file? y/n
y

Fetching management.template.cfg...
management.template.cfg successfully downloaded

Please fill the configuration file and run this script again.
```

### 2.1 .cfg arguments

| **Argument**            | **Description**                                                                                                                                                                                                                                                                 | **Example**                          |
|-------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------|
| **FC_TAG**              | Tag/version of the FluxConfig.Management image which will be used. <br> You can find all tags [here](https://hub.docker.com/r/fluxconfig/fluxconfig.management/tags)                                                                                                            | 1.0-pre                              |
| **MANAGEMENT_API_PORT** | Port for api, which will be exposed from container                                                                                                                                                                                                                              | Any free port, e.g 7070              |
| **FCWC_URL**            | Address of FluxConfig.WebClient for CORS configuration                                                                                                                                                                                                                          | http://fcwebclient:3000              |
| **FCS_BASE_URLL**       | Address of FluxConfig.Storage service secured with TLS                                                                                                                                                                                                                          | https://fcstorage:8080               |
| **FC_API_KEY**          | Internal api-key for interservice communication. <br> If you don't have one, leave this argument empty and it will be generated for you. <br> Remember it for deployment the rest of the FluxConfig system. <br> If you already have one from other deployments - fill argument | e72eb4601d5a45bd9d5fd8b439b9097f |
| **FCM_SYSADMIN_EMAIL**   | Email of the account that will be granted system administrator rights during system initialization. Can be changed after installation                                                                                                                                           | admin@gmail.com                      |
| **FCM_SYSADMIN_USERNAME**  | Username of the account that will be granted system administrator rights during system initialization. Can be changed after installation                                                                                                                                        | FluxAdmin                            |
| **FCM_SYSADMIN_PASSWORD**   | Password of the account that will be granted system administrator rights during system initialization. Can be changed after installation                                                                                                                                        | 12345678                             |
| **PG_USER**             | Username for internal PostgreSQL connection, fill it or leave empy for auto generation                                                                                                                                                                                          | PgUser                               |
| **PG_PSWD**             | Password for internal PostgreSQL connection, fill it or leave empy for auto generation                                                                                                                                                                                          | PgPassword                           |

### 3. Execute deployment script

**Give executable permissions to the file**

```bash
chmod +x boot_management.sh
```

**Deploy**

```bash
./boot_management.sh -c "PATH TO YOUR .cfg FILE"
```

**Example of output for successful deployment**

```bash
Fetching docker-compose.yml...
docker-compose.yml successfully downloaded

Reading .cfg file...
Loaded: FC_TAG=1.0-pre
Loaded: MANAGEMENT_API_PORT=7070
Loaded: FCWC_URL=http://host.docker.internal:3000
Loaded: FCS_BASE_URL=https://host.docker.internal:7045
Loaded: FCM_SYSADMIN_EMAIL=admin@gmail.com
Loaded: FCM_SYSADMIN_USERNAME=FluxAdmin
Loaded: FCM_SYSADMIN_PASSWORD=12345
Loaded: PG_USER=exampleuser
Loaded: PG_PSWD=12345

Generating value for FC_API_KEY
Loaded: FC_API_KEY=e72eb4601d5a45bd9d5fd8b439b9097f

Generating Postgres FCM application User
Generating Postgres migration User
Generating Postgres FCM application Password
Generating Postgres migration Password

Starting FluxConfig Management...
[+] Running 2/2
 ✔ fcm_database Pulled
 ✔ management_api Pulled 
[+] Running 4/4
 ✔ Network deployment_fcm-network   Created 
 ✔ Volume "deployment_fcm-pg-data"  Created 
 ✔ Container fc-postgres            Started 
 ✔ Container fc-management          Started 
```