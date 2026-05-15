################################################## methods ##################################################

copy_to_github() {
  echo "Syncing $1"
  # Syncs files recursively, deletes extra files at destination, excludes specified patterns
  rsync --recursive --delete ado/$1 github \
    --exclude '.git' \
    --exclude 'node_modules' \
    --exclude 'tmp' \
    --exclude '.venv'
 
  cd github/$1
  git add .
  git status
  git commit -m "Update on $(date +'%Y-%m-%d')"
  git push
}

################################################## execute ##################################################

clear
copy_to_github Projects
  
echo "Done..."
sleep 3
