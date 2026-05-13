#!/bin/bash

################################################## variables ##################################################

ado=(
  # https://dev.azure.com/{COMPANY}/{PROJECT}/_apis/git/repositories
  "Projects"
)
github=(
  "Projects"
)

################################################## methods ##################################################

# $1 = git folder path
# $2 = local folder path
# $3 = array of projects
git_daily() {
  local gitFolder=$1
  local localFolder=$2
  shift 2 # remove first two arguments
  local projectList=("$@") # get an array for rest of the arguments

  if [ ! -d $localFolder ]; then # create the directory if it does not exist
    mkdir $localFolder
  fi

  for project in ${projectList[@]}; do
    echo $localFolder/$project
    if [ ! -d $localFolder/$project ]; then # clone the repo for the first time
      git clone $gitFolder/$project $localFolder/$project
    else # update the repo
      git -C $localFolder/$project branch # list local branches
      git -C $localFolder/$project remote prune origin # remove local branches which were deleted from origin

      # delete local branches that no longer exists in origin
      git -C $localFolder/$project branch -vv | # list local branches with ': gone]' if it does not exist
        grep ': gone]' | # get the gone branches
        grep -v "\*" | # ignore the gone branch if that branch is currently selected
        awk '{ print $1; }' |  # get the branch name
        xargs -r git -C $localFolder/$project branch -D # remove the branch

      # pull the latest
      git -C $localFolder/$project pull --quiet
    fi
  done
}

################################################## execute ##################################################

clear
git_daily "git@ssh.dev.azure.com:v3/{COMPANY}/{PROJECT}" "ado" ${ado[@]}
git_daily "git@github.com:{ACCOUNT}" "github" ${github[@]}

echo "Done..."
sleep 3
# read # keeps the terminal open until enter is clicked
