package main

import (
	"encoding/json"
	"log"
	"net/http"

	"zuhid.com/auth/handler"
)

type JSONHandler func(r *http.Request) (interface{}, error)

func JsonMiddleware(next JSONHandler) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		data, err := next(r)

		// Set JSON header once globally
		w.Header().Set("Content-Type", "application/json")

		if err != nil {
			w.WriteHeader(http.StatusBadRequest)
			json.NewEncoder(w).Encode(map[string]string{
				"error": err.Error(),
			})
			return
		}
		w.WriteHeader(http.StatusOK)
		json.NewEncoder(w).Encode(data)
	}
}

func main() {
	http.HandleFunc("/account", JsonMiddleware(handler.AccountHandler))

	port := "127.0.0.1:8080"
	log.Println("Server running on http://" + port)
	http.ListenAndServe(port, nil)
}
