function p = samebirthday(n=23,N=1000)
  count = 0;
  for i=1:N
    birthdays = randi(365,1,n);
    if length(unique(birthdays))<n
      count++;
    endif
  endfor
  p=count/N;
end

% randi(maxim, dim1, dim2) - genereaza un vector de dim1xdim2, 
% cu valori intre 1 si maxim
% randi(maxim) - genereaza un nr intre 1 si maxim