# inotifywait --monitor --event modify **/*.go |
inotifywait --monitor --event modify main.go |
while read file event; do
    clear # clear the console
    go run . # run the code
    # go build -o tmp/generator.o
    # ./tmp/generator.o
done
