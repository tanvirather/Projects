# Install Software on Ubuntu
```sh
sudo apt update 
sudo apt upgrade --yes
sudo apt install --yes build-essential curl git inotify-tools

######################### [Install vscode](https://code.visualstudio.com/Download)
sudo snap install --classic code 
wget -qO- https://raw.githubusercontent.com/harry-cpp/code-nautilus/master/install.sh | bash # Install "Open in Code" context menu

######################### Install Chrome
wget https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb
sudo apt install ./google-chrome-stable_current_amd64.deb

######################### Microsoft Edge
https://www.microsoft.com/en-us/edge/download
sudo dpkg -i microsoft-edge-stable_*************_amd64.deb 

######################### Install rclone
sudo apt install rclone
rclone config

######################### Install keepassxc
sudo snap install keepassxc

######################### Create ssh key (https://docs.microsoft.com/en-us/azure/devops/repos/git/use-ssh-keys-to-authenticate?view=azure-devops)

ssh-keygen -t rsa -b 4096 -f ~/.ssh/id_zuhid -C "tanvir.ather@zuhid.com"
git config --global user.name "Tanvir Ather"
git config --global user.email "tanvir.ather@zuhid.com"

######################### go (https://go.dev/doc/install)
sudo apt install --yes golang-go
sudo apt install --yes got
# go version

######################### Dotnet
sudo apt install --yes dotnet-sdk-10.0
# dotnet --list-sdks # verify

######################### nodejs and npm Begin (https://nodejs.org/en/download)
curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.4/install.sh | bash # Download and install nvm:
\. "$HOME/.nvm/nvm.sh" # in lieu of restarting the shell
nvm install 24 # Download and install Node.js:
node -v # # Verify the Node.js version: Should print "v24.15.0".
npm -v # # Verify npm version: Should print "11.12.1".

######################### teams
sudo snap install teams-for-linux

######################### Docker (https://docs.docker.com/engine/install/ubuntu/#install-using-the-repository)
# Add Docker's official GPG key:
sudo apt update
sudo apt install ca-certificates curl
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc

# Add the repository to Apt sources:
sudo tee /etc/apt/sources.list.d/docker.sources <<EOF
Types: deb
URIs: https://download.docker.com/linux/ubuntu
Suites: $(. /etc/os-release && echo "${UBUNTU_CODENAME:-$VERSION_CODENAME}")
Components: stable
Signed-By: /etc/apt/keyrings/docker.asc
EOF

sudo apt update

# Install the latest version
sudo apt install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin --yes

# allow docker to be run wihtout sudo
sudo groupadd docker
sudo usermod -aG docker $USER







######################### Azure Cli Begin (https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-apt)
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

######################### Python
sudo apt update
sudo apt install --yes python3 python3-venv python3-pip
# python3 --version # verify

######################### postgresql-client tools
sudo apt install --yes postgresql-client

######################### Install Office
# sudo apt install --yes libreoffice
sudo apt install --yes libreoffice-writer
sudo apt install --yes libreoffice-calc

######################### Scribus - pdf editor (https://linuxhint.com/install-scribus-ubuntu)
sudo add-apt-repository ppa:scribus/ppa
sudo apt update --yes
sudo apt install --yes scribus

######################### OBS
sudo add-apt-repository ppa:obsproject/obs-studio
sudo apt update --yes
sudo apt install --yes obs-studio

######################### VirtualBox (https://www.virtualbox.org/wiki/Linux_Downloads)
wget https://download.virtualbox.org/virtualbox/7.2.4/virtualbox-7.2_7.2.4-170995~Ubuntu~noble_amd64.deb
sudo apt install ./virtualbox-7.2_7.2.4-170995~Ubuntu~noble_amd64.deb

######################### kubernetes (https://kubernetes.io/docs/tasks/tools)
# kubectl: https://kubernetes.io/docs/tasks/tools/install-kubectl-linux)
curl -LO "https://dl.k8s.io/release/$(curl -L -s https://dl.k8s.io/release/stable.txt)/bin/linux/amd64/kubectl" # Download the latest release
sudo install -o root -g root -m 0755 kubectl /usr/local/bin/kubectl # Install kubectl
# kubectl version --client # verify

######################### minikube (https://minikube.sigs.k8s.io/docs/start)
curl -LO https://github.com/kubernetes/minikube/releases/latest/download/minikube-linux-amd64 # Download the latest release
sudo install minikube-linux-amd64 /usr/local/bin/minikube && rm minikube-linux-amd64 # Install minikube
minikube start
minikube addons enable metrics-server 
minikube addons enable ingress
minikube addons list

######################### Cleanup
sudo apt update --yes
sudo apt upgrade
sudo apt autoremove --yes
```


