package socialnetwork.service;

import socialnetwork.domain.*;
import socialnetwork.domain.utils.Observable;
import socialnetwork.repository.database.db.EventDbRepository;
import socialnetwork.repository.database.db.Repository;
import socialnetwork.domain.utils.Graph;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.StreamSupport;

import static java.lang.Math.toIntExact;

/**
 * Service class
 */
public class Service implements Observable {

    private final Repository<Long, User> usersRepo;
    private final Repository<Tuple<User, User>, Friendship> friendshipsRepo;
    private final Repository<Tuple<User, User>, FriendRequest> friendRequestsRepo;
    private final Repository<Long, Message> messageRepository;
    private final EventDbRepository eventRepository;

    /**
     * constructor
     * @param usersRepo - Repository<Long, User>
     * @param friendshipsRepo - Repository<Tuple<User, User>, Friendship>
     */
    public Service(Repository<Long, User> usersRepo, Repository<Tuple<User, User>, Friendship> friendshipsRepo,
                Repository<Long, Message> messageRepository, Repository<Tuple<User, User>, FriendRequest> friendRequestsRepository,
                EventDbRepository eventRepository) {
        this.usersRepo = usersRepo;
        this.friendshipsRepo = friendshipsRepo;
        this.messageRepository = messageRepository;
        this.friendRequestsRepo = friendRequestsRepository;
        this.eventRepository = eventRepository;
    }

    /**
     * adds an user
     * @param firstName - String
     * @param lastName - String
     * @return the saved user
     */
    public User addUser(String firstName, String lastName) {
        return usersRepo.save(new User(firstName, lastName));
    }

    /**
     * removes an user
     * @param id - User
     * @return the removed user
     */
    public User removeUser(Long id) {
        User user = usersRepo.findOne(id);
        Iterable<User> list = usersRepo.findAll();
        for (User u: list) {
            u.removeFriend(user);
            removeFriendship(u.getId(), user.getId());
            removeFriendship(user.getId(),u.getId());
        }
        return usersRepo.delete(user.getId());
    }

    /**
     * adds a friendship
     * @param u1 - long
     * @param u2 - long
     * @return friendship between users with ids u1 ans u2
     */
    public Friendship addFriendship(long u1, long u2) {
        boolean b = usersRepo.findOne(u1).addFriend(usersRepo.findOne(u2));
        boolean a = usersRepo.findOne(u2).addFriend(usersRepo.findOne(u1));
        if(a && b)
        {
            Friendship f = new Friendship(usersRepo.findOne(u1), usersRepo.findOne(u2));
            friendshipsRepo.save(f);
            this.update();
            return f;
        }
        return null;
    }

    /**
     * removes a friendship
     * @param u1 - long
     * @param u2 - long
     * @return the removed friendship
     */
    public Friendship removeFriendship(long u1, long u2) {
        usersRepo.findOne(u1).removeFriend(usersRepo.findOne(u2));
        usersRepo.findOne(u2).removeFriend(usersRepo.findOne(u1));
        Friendship f = friendshipsRepo.delete(new Tuple<>(usersRepo.findOne(u1), usersRepo.findOne(u2)));
        this.update();
        return f;
    }

    /**
     * returns all users
     * @return users list
     */
    public Iterable<User> getAllUsers(){
        Iterable<User> users = usersRepo.findAll();
        users.forEach(u -> {
            for (Friendship f: friendshipsRepo.findAll())
                if(f.getUser1().getId().equals(u.getId()))
                    u.addFriend(usersRepo.findOne(f.getUser2().getId()));
                else if(f.getUser2().getId().equals(u.getId()))
                    u.addFriend(usersRepo.findOne(f.getUser1().getId()));
            });
        return users;
    }

    /**
     * returns all friendships
     * @return friendships list
     */
    public Iterable<Friendship> getAllFriendships(){
        return friendshipsRepo.findAll();
    }

    /**
     * returns the number of communities in the social network
     * @return int
     */
    public  int communitiesNumber() {
        int size = 0;
        for (User u: usersRepo.findAll()) {
            size = toIntExact(u.getId());
        }
        Graph g = new Graph(size);
        for (Friendship f : friendshipsRepo.findAll())
                g.addEdge(toIntExact(f.getUser1().getId()), toIntExact(f.getUser2().getId()));
        g.DFS();
        int nr = g.connectedComponents();
        nr = nr - (size - usersRepo.size());
        return nr;
    }

    /**
     * returns a list of ints representing the most sociable community
     * @return List<Integer>
     */
    public List<Integer> mostSociableCommunity() {
        int size = 0;
        for (User u: usersRepo.findAll())
            size = toIntExact(u.getId());
        Graph g = new Graph(size);
        for (Friendship f : friendshipsRepo.findAll())
                g.addEdge(toIntExact(f.getUser1().getId()), toIntExact(f.getUser2().getId()));
        g.DFS();
        int max = 1;
        List<Integer> comp = new ArrayList<>();
        ArrayList<ArrayList<Integer> > lists = g.returnComponents();
        for (ArrayList<Integer> list:lists)
            if(list.size() >= max) {
                max = list.size();
                comp = list;
            }
        return comp;
    }


    public List<Friendship> reportUserFriends(Long id) {
        if(StreamSupport.stream(getAllUsers().spliterator(), false)
                .collect(Collectors.toList()).stream().noneMatch(y -> y.getId().equals(id))) {
            System.out.println("No user");
            return null;
        }
        else {
            return StreamSupport.stream(getAllFriendships().spliterator(), false)
                    .collect(Collectors.toList()).stream()
                    .filter(friendship -> friendship.getUser1().getId().equals(id) ||
                            friendship.getUser2().getId().equals(id))
                    .collect(Collectors.toList());
    }}

    public Tuple<User, List<User>> reportUsersFriendsMonth(Long id, Integer month){
        User user = usersRepo.findOne(id);
        String regex = ".{4}-0{0,1}+" + month.toString() + "-[0-9]{1,2}";
        List<User> list = StreamSupport.stream(friendshipsRepo.findAll().spliterator(), false)
                .filter(f -> f.getUser1().getId().equals(id) || f.getUser2().getId().equals(id))
                .filter(f -> f.getDate().toString().matches(regex))
                .map(f -> {
                    if(f.getUser1().getId().equals(id))
                        return f.getUser2();
                    else
                        return f.getUser1();
                })
                .collect(Collectors.toList());
        return new Tuple<>(user, list);
    }

    public Tuple<User, List<Message>> reportUsersMessagesMonth(Long id, Integer month){
        User user = usersRepo.findOne(id);
        List<Message> list = getInbox(user).stream()
                .filter(m -> m.getDate().getMonth().getValue() == month)
                .collect(Collectors.toList());
        return new Tuple<>(user, list);
    }

    public Tuple<User, List<Message>> reportUsersMessagesFriendMonth(Long id, Long idFriend, Integer month){
        User user = usersRepo.findOne(id);
        List<Message> list = getInbox(user).stream()
                .filter(m -> m.getDate().getMonth().getValue() == month)
                .filter(m -> m.getFrom().getId().equals(idFriend))
                .collect(Collectors.toList());
        return new Tuple<>(user, list);
    }

    public List<FriendRequest> getUserFriendRequests(Long id){
        return StreamSupport.stream(friendRequestsRepo.findAll().spliterator(), false)
                .filter(fr -> fr.getTo().getId().equals(id))
                .collect(Collectors.toList());
    }

    public User getUser(Long id){
        User user = usersRepo.findOne(id);
        StreamSupport.stream(getAllFriendships().spliterator(), false)
                .filter(fr -> fr.getUser1().getId().equals(id) || fr.getUser2().getId().equals(id))
                .map(fr -> {
                    if(fr.getUser1().getId().equals(id))
                        return fr.getUser2();
                    else
                        return fr.getUser1();
                })
                .forEach(user::addFriend);
        return user;
    }

    public FriendRequest addFriendRequest(FriendRequest fr){
        FriendRequest temp = friendRequestsRepo.findOne(new Tuple<>(fr.getFrom(), fr.getTo()));
        if(temp != null)
            if(temp.getStatus().matches("rejected") ||
                    (temp.getStatus().matches("approved") &&
                            friendshipsRepo.findOne(new Tuple<>(fr.getFrom(), fr.getTo())) == null)) {
                FriendRequest f = friendRequestsRepo.update(fr);
                this.update();
                return f;
            }
            else
                return null;

        temp = friendRequestsRepo.findOne(new Tuple<>(fr.getTo(), fr.getFrom()));
        if(temp != null)
            if(temp.getStatus().matches("rejected")) {
                FriendRequest f = friendRequestsRepo.save(fr);
                this.update();
                return f;
            }
            else
                return null;
        FriendRequest f = friendRequestsRepo.save(fr);
        this.update();
        return f;
    }

    public Friendship acceptFriendRequest(User to, User from){
        List<FriendRequest> requestList = getUserFriendRequests(to.getId())
                .stream()
                .filter(fr -> fr.getFrom().getId().equals(from.getId()))
                .collect(Collectors.toList());

        if(requestList.isEmpty())
            return null;

        FriendRequest fr = requestList.get(0);
        if(!fr.getStatus().matches("pending"))
            return null;

        fr.setStatus("approved");
        friendRequestsRepo.update(fr);

        Friendship f = addFriendship(to.getId(), from.getId());
        this.update();
        return f;
    }

    public boolean rejectFriendRequest(User to, User from){
        List<FriendRequest> requestList = getUserFriendRequests(to.getId())
                .stream()
                .filter(fr -> fr.getFrom().getId().equals(from.getId()))
                .collect(Collectors.toList());

        if(requestList.isEmpty())
            return false;

        FriendRequest fr = requestList.get(0);
        if(!fr.getStatus().matches("pending"))
            return false;

        fr.setStatus("rejected");
        friendRequestsRepo.update(fr);
        this.update();

        return true;
    }

    public List<Message> getChats(Long id1, Long id2) {
        List<Message> list = new ArrayList<>();

        for (Message m: messageRepository.findAll()) {
            if(m.getFrom().getId().equals(id1))
                for (User u: m.getTo())
                    if(u.getId().equals(id2))
                        list.add(m);

            if(m.getFrom().getId().equals(id2))
                for (User u: m.getTo())
                    if(u.getId().equals(id1))
                        list.add(m);
        }
        return  list;
    }

    public List<Message> getChatsPagination(Long id1, Long id2, int t) {
        List<Message> list = new ArrayList<>();

        for (Message m: messageRepository.findAllPagination(t, id1, id2)) {
            if(m.getFrom().getId().equals(id1))
                for (User u: m.getTo())
                    if(u.getId().equals(id2))
                        list.add(m);

            if(m.getFrom().getId().equals(id2))
                for (User u: m.getTo())
                    if(u.getId().equals(id1))
                        list.add(m);
        }
        return  list;
    }

    public List<Message> getInbox(User user) {
        List<Message> list = new ArrayList<>();

        for (Message m: messageRepository.findAll())
            for(User to : m.getTo())
                if(to.getId().equals(user.getId()))
                    list.add(m);

        return  list;
    }

    public void sendMessage(Message m){
        messageRepository.save(m);
        this.update();
    }

    public void replyAll(Message m, User user, String reply) {
        ArrayList<User> toList = new ArrayList<>();

        if(!m.getFrom().getId().equals(user.getId()))
            toList.add(m.getFrom());
        for(User u : m.getTo())
            if(!u.getId().equals(user.getId()))
                toList.add(u);
        Message message = new Message(user, toList, reply);
        message.setReply(m);
        messageRepository.save(message);
        this.update();
    }

    public Repository<Long, User> getUserRepo() {
        return this.usersRepo;
    }

    public List<FriendRequest> getUserSentFriendRequests(Long id) {
        return StreamSupport.stream(friendRequestsRepo.findAll().spliterator(), false)
                .filter(fr -> fr.getFrom().getId().equals(id))
                .collect(Collectors.toList());
    }

    public FriendRequest removeFriendRequest(User fromUser, User toUser) {
        List<FriendRequest> requestList = getUserFriendRequests(toUser.getId())
                .stream()
                .filter(fr -> fr.getFrom().getId().equals(fromUser.getId()))
                .collect(Collectors.toList());

        if(requestList.isEmpty())
            return null;

        FriendRequest fr = requestList.get(0);
        if(!fr.getStatus().matches("pending"))
            return null;

        fr = friendRequestsRepo.delete(new Tuple<>(fromUser, toUser));
        this.update();
        return fr;
    }

    public Message getMessage(Long id){
        return messageRepository.findOne(id);
    }

    public Event addEvent(String name, LocalDate date){
        Event event = new Event(name, date);
        Event result = eventRepository.save(event);
        this.update();
        return result;
    }

    public Iterable<Event> findAllEvents(){
        return eventRepository.findAll();
    }

    public void subscribeToEvent(Long eventId, Long userId){
        eventRepository.subscribe(eventId, userId);
        this.update();
    }

    public void unsubscribeFromEvent(Long eventId, Long userId){
        eventRepository.unsubscribe(eventId, userId);
        this.update();
    }
}
