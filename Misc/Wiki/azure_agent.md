# Download
```sh
mkdir -p azure_agent
cd azure_agent
wget "https://download.agent.dev.azure.com/agent/4.272.0/vsts-agent-linux-x64-4.272.0.tar.gz" -O "vsts-agent-linux-x64-4.272.0.tar.gz"
tar zxvf "vsts-agent-linux-x64-4.272.0.tar.gz"
```

# Configure
```sh
./config.sh remove # to remove
./config.sh
Enter server URL > https://dev.azure.com/ORGANIZATION
PAT > Create one in https://dev.azure.com/ORGANIZATION/_usersSettings/tokens with "Agent Pools: Read & manage"
Enter agent name > COMPUTER_NAME

sudo ./svc.sh install
```

# run
```sh
sudo ./svc.sh start
sudo ./svc.sh status
sudo ./svc.sh stop
sudo ./svc.sh uninstall
```
