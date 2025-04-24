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
| **FCWC_URL**            |                                                                                                                                                                                                                                                                                 | http://fcwebclient:3000              |
| **FCS_BASE_URLL**       | URL to FluxConfig.Storage service secured with TLS                                                                                                                                                                                                                              | https://fcstorage:8080               |
| **FC_API_KEY**          | Internal api-key for interservice communication. <br> If you don't have one, leave this argument empty and it will be generated for you. <br> Remember it for deployment the rest of the FluxConfig system. <br> If you already have one from other deployments - fill argument | 3CB73B25-8A67-497D-85C9-CA84DD5C7A79 |
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
                                                                                                   0.3s 
```