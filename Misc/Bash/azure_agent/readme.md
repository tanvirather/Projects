# [Run a self-hosted agent in Docker](https://learn.microsoft.com/en-us/azure/devops/pipelines/agents/docker?view=azure-devops#linux)

## Create "Personal Access Token" (PAT) with following permissions
- Agent Pools: Read & manage

## Create a file "secrets.env" with the following content
```
AZP_URL=https://dev.azure.com/COMPANY_NAME
AZP_AGENT_NAME=[USER_NAME.MACHINE_NAME]
AZP_TOKEN=[REPACE_WITH_AZP_TOKEN]
```

## Build the image and run the container
```
execute the file "run.sh" to build and run the container
```

## Helpfull commands
```sh
docker exec -it dockeragent bash # execute into the container
```
