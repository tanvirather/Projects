package handler

import (
	"fmt"
	"net/http"

	"zuhid.com/auth/model"
	"zuhid.com/auth/repository"
)

func AccountHandler(r *http.Request) (any, error) {
	var model *model.Account
	switch r.Method {
	case http.MethodGet:
		model = get()
	case http.MethodPost:
		add()
	case http.MethodPut:
		update()
		// default:
		// 	http.Error(w, "", http.StatusMethodNotAllowed)
	}
	return model, nil
}

func get() *model.Account {
	return repository.AccountRepository{}.GetAccount()
}

func add() {
	fmt.Println("POST create user")
}

func update() {
	fmt.Println("PUT update user")
}
