package repository

import (
	"github.com/google/uuid"
	"zuhid.com/auth/model"
)

type AccountRepository struct {
}

func (r AccountRepository) GetAccount() *model.Account {
	return &model.Account{
		Id:    uuid.New(),
		Email: "test@company.com",
	}
}
