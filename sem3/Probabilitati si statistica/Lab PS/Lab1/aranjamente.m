function result = aranjamente(v, k)
  combination = nchoosek(v,k); % combinari de n luate cate k din vectorul v
  result = [];
  for row = 1:rows(combination) % rows(ceva) - nr de randuri
    result = [result;perms(combination(row,:))]; % ; e un fel de + pt concatenare
  endfor
  result=sortrows(result);
end